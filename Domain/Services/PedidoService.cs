using AutoMapper;
using Shared.Dtos.Request;
using Shared.Dtos.Response;
using Domain.Entities;
using Domain.Interfaces;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Domain.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IRepository<PedidoDataModel, Pedido> _pedidoRepository;
        private readonly bool isReformaTributariaAtiva = true; // Feature flag para cálculo de imposto
        private readonly IMapper _mapper;
        private readonly ILogger<PedidoService> _logger;

        public PedidoService(IRepository<PedidoDataModel, Pedido> pedidoRepository, IMapper mapper, ILogger<PedidoService> logger)
        {
            _pedidoRepository = pedidoRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReceberPedidoResponse> CriarPedidoAsync(PedidoRequestDto pedidoRequestDto)
        {
            try
            {
                _logger.LogInformation("Iniciando criação do pedido {PedidoId}", pedidoRequestDto.PedidoId);

                // Verificar se o pedido já existe para evitar duplicidade
                var pedidoExistente = await _pedidoRepository.GetPedidoByIdWithItemsAsync(pedidoRequestDto.PedidoId);
                if (pedidoExistente != null)
                {
                    _logger.LogWarning("Pedido duplicado detectado: {PedidoId}", pedidoRequestDto.PedidoId);

                    return new ReceberPedidoResponse
                    {
                        Mensagem = "Pedido duplicado: Já existe um pedido com o mesmo ID."
                    };
                }

                // Buscar pedidos que contenham os mesmos itens
                var todosPedidos = await _pedidoRepository.GetTodosPedidosAsync();
                var possuiItensDuplicados = todosPedidos.Any(p =>
                    p.ItensPedidos.Count == pedidoRequestDto.Itens.Count && // Mesma quantidade de itens
                    p.ItensPedidos.All(itemExistente =>
                        pedidoRequestDto.Itens.Any(itemNovo =>
                            itemNovo.ProdutoId == itemExistente.ProdutoId &&
                            itemNovo.Quantidade == itemExistente.Quantidade &&
                            itemNovo.Valor == itemExistente.Valor)
                    )
                );

                if (possuiItensDuplicados)
                {
                    _logger.LogWarning("Pedido duplicado: Já existe um pedido com os mesmos itens. {PedidoId}", pedidoRequestDto.PedidoId);
                    return new ReceberPedidoResponse
                    {
                        Mensagem = "Pedido duplicado: Já existe um pedido com os mesmos itens."
                    };
                }

                // Calcular o valor total dos itens
                var totalItens = pedidoRequestDto.Itens.Sum(i => i.Valor * i.Quantidade);

                // Calcular o imposto com base na feature flag
                var imposto = isReformaTributariaAtiva ? totalItens * 0.2m : totalItens * 0.3m;

                var novoPedido = new PedidoDataModel
                {
                    PedidoId = pedidoRequestDto.PedidoId,
                    ClienteId = pedidoRequestDto.ClienteId,
                    Status = "Criado",
                    DataPedido = DateTime.Now,
                    Imposto = imposto,
                    ItensPedidos = pedidoRequestDto.Itens.Select(i => new ItensPedidoDataModel
                    {
                        ProdutoId = i.ProdutoId,
                        Quantidade = i.Quantidade,
                        Valor = i.Valor
                    }).ToList()
                };

                await _pedidoRepository.CriaPedidoAsync(novoPedido);
                _logger.LogInformation("Pedido {PedidoId} criado com sucesso.", novoPedido.PedidoId);

                var response = new ReceberPedidoResponse
                {
                    Id = novoPedido.PedidoId,
                    Status = novoPedido.Status
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar pedido {PedidoId}", pedidoRequestDto.PedidoId);
                throw new Exception("Erro inesperado ao criar o pedido.", ex);
            }
        }

        public async Task<ConsultarPedidoResponse> ConsultarPedidoAsync(int id)
        {
            _logger.LogInformation("Iniciando consulta do pedido {PedidoId}", id);

            try
            {
                var pedido = await _pedidoRepository.GetPedidoByIdWithItemsAsync(id);

                if (pedido == null)
                {
                    _logger.LogWarning("Pedido {PedidoId} não encontrado.", id);
                    throw new KeyNotFoundException("Pedido não encontrado.");
                }

                _logger.LogInformation("Pedido {PedidoId} encontrado com sucesso.", id);
                return _mapper.Map<ConsultarPedidoResponse>(pedido);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError("Erro ao consultar pedido {PedidoId}: {Mensagem}", id, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao consultar pedido {PedidoId}.", id);
                throw new Exception("Erro inesperado ao consultar o pedido.", ex);
            }
        }

        public async Task<List<ConsultarPedidoResponse>> ListarPedidosPorStatusAsync(string status)
        {
            _logger.LogInformation("Iniciando listagem de pedidos com status: {Status}", status);

            try
            {
                var pedidos = await _pedidoRepository.GetTodosPedidosAsync();

                var pedidosFiltrados = pedidos
                    .Where(p => p.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                    .Select(p => new ConsultarPedidoResponse
                    {
                        PedidoId = p.PedidoId,
                        ClienteId = p.ClienteId,
                        Imposto = p.Imposto,
                        Status = p.Status,
                        DataPedido = p.DataPedido,
                        Itens = p.ItensPedidos.Select(i => new ItemPedidoResponse
                        {
                            ProdutoId = i.ProdutoId,
                            Quantidade = i.Quantidade,
                            Valor = i.Valor
                        }).ToList()
                    }).ToList();

                _logger.LogInformation("{Quantidade} pedidos encontrados com status {Status}.", pedidosFiltrados.Count, status);
                return pedidosFiltrados;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao listar pedidos com status {Status}.", status);
                throw new Exception("Erro inesperado ao listar os pedidos por status.", ex);
            }
        }
    }
}