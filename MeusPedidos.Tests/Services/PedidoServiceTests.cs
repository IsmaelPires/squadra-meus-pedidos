using AutoMapper;
using Domain.Services;
using Domain.Entities;
using Data.Interfaces;
using Shared.Dtos.Request;
using Shared.Dtos.Response;
using Data.Models;

public class PedidoServiceTests
{
    private readonly PedidoService _pedidoService;
    private readonly IRepository<PedidoDataModel, Pedido> _pedidoRepository = Substitute.For<IRepository<PedidoDataModel, Pedido>>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly Faker _faker = new();

    public PedidoServiceTests()
    {
        _pedidoService = new PedidoService(_pedidoRepository, _mapper);
    }

    [Fact]
    public async Task CriarPedidoAsync_DeveRetornarErro_SePedidoJaExistir()
    {
        var pedidoRequest = new PedidoRequestDto { PedidoId = 1, Itens = new List<ItemPedidoRequestDto>() };
        _pedidoRepository.GetPedidoByIdWithItemsAsync(pedidoRequest.PedidoId).Returns(new ConsultarPedidoResponse());

        var resultado = await _pedidoService.CriarPedidoAsync(pedidoRequest);

        resultado.Mensagem.Should().Be("Pedido duplicado: Já existe um pedido com o mesmo ID.");
    }

    [Fact]
    public async Task CriarPedidoAsync_DeveRetornarErro_SeItensDuplicados()
    {
        var pedidoRequest = new PedidoRequestDto
        {
            PedidoId = 2,
            Itens = new List<ItemPedidoRequestDto>
            {
                new ItemPedidoRequestDto { ProdutoId = 1, Quantidade = 2, Valor = 10 }
            }
        };

        var pedidoExistente = new PedidoDataModel
        {
            PedidoId = 99,
            ItensPedidos = new List<ItensPedidoDataModel>
            {
                new ItensPedidoDataModel { ProdutoId = 1, Quantidade = 2, Valor = 10 }
            }
        };

        _pedidoRepository.GetTodosPedidosAsync().Returns(new List<PedidoDataModel> { pedidoExistente });

        var resultado = await _pedidoService.CriarPedidoAsync(pedidoRequest);

        resultado.Mensagem.Should().Be("Pedido duplicado: Já existe um pedido com os mesmos itens.");
    }

    [Fact]
    public async Task CriarPedidoAsync_DeveCriarPedido_ComSucesso()
    {
        var pedidoRequest = new PedidoRequestDto
        {
            PedidoId = 3,
            ClienteId = _faker.Random.Int(1, 100),
            Itens = new List<ItemPedidoRequestDto>
            {
                new ItemPedidoRequestDto { ProdutoId = 1, Quantidade = 1, Valor = 50 }
            }
        };

        _pedidoRepository.GetPedidoByIdWithItemsAsync(pedidoRequest.PedidoId).Returns((ConsultarPedidoResponse)null);
        _pedidoRepository.GetTodosPedidosAsync().Returns(new List<PedidoDataModel>());

        var resultado = await _pedidoService.CriarPedidoAsync(pedidoRequest);

        resultado.Status.Should().Be("Criado");
        resultado.Id.Should().Be(pedidoRequest.PedidoId);
    }

    [Fact]
    public async Task ConsultarPedidoAsync_DeveRetornarPedido_SeExistir()
    {
        var pedidoId = _faker.Random.Int(1, 100);
        var pedidoMock = new ConsultarPedidoResponse { PedidoId = pedidoId, Status = "Criado" };

        _pedidoRepository.GetPedidoByIdWithItemsAsync(pedidoId).Returns(pedidoMock);
        _mapper.Map<ConsultarPedidoResponse>(pedidoMock).Returns(new ConsultarPedidoResponse { PedidoId = pedidoId });

        var resultado = await _pedidoService.ConsultarPedidoAsync(pedidoId);

        resultado.PedidoId.Should().Be(pedidoId);
    }

    [Fact]
    public async Task ConsultarPedidoAsync_DeveLancarExcecao_SeNaoEncontrado()
    {
        var pedidoId = _faker.Random.Int(1, 100);
        _pedidoRepository.GetPedidoByIdWithItemsAsync(pedidoId).Returns((ConsultarPedidoResponse)null);

        Func<Task> act = async () => await _pedidoService.ConsultarPedidoAsync(pedidoId);

        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Pedido não encontrado.");
    }

    [Fact]
    public async Task ListarPedidosPorStatusAsync_DeveRetornarPedidosFiltrados()
    {
        var status = "Criado";
        var pedidos = new List<PedidoDataModel>
        {
            new PedidoDataModel { PedidoId = 1, ClienteId = 10, Status = "Criado" },
            new PedidoDataModel { PedidoId = 2, ClienteId = 20, Status = "Finalizado" },
        };

        _pedidoRepository.GetTodosPedidosAsync().Returns(pedidos);

        var resultado = await _pedidoService.ListarPedidosPorStatusAsync(status);

        resultado.Should().HaveCount(1);
        resultado.First().Status.Should().Be(status);
    }
}