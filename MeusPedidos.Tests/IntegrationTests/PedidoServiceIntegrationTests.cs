using AutoMapper;
using Domain.Entities;
using Domain.Services;
using Data.Interfaces;
using Data.Models;
using Shared.Dtos.Request;
using Shared.Dtos.Response;
using Testcontainers.MsSql;
using Microsoft.Data.SqlClient;

public class PedidoServiceIntegrationTests : IAsyncLifetime
{
    private readonly PedidoService _pedidoService;
    private readonly IRepository<PedidoDataModel, Pedido> _pedidoRepository;
    private readonly IMapper _mapper;
    private readonly MsSqlContainer _sqlServerContainer;
    private readonly string _connectionString;

    public PedidoServiceIntegrationTests()
    {
        // Configuração do TestContainer para SQL Server
        _sqlServerContainer = new MsSqlBuilder()
            .WithPassword("TestPassword123") // Define a senha obrigatória
            .WithPortBinding(1433, 1433)     // Porta exposta no host
            .Build();

        // Obtém a string de conexão
        _connectionString = _sqlServerContainer.GetConnectionString();

        // Configurar o AutoMapper (se necessário, ajuste o perfil conforme necessário)
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PedidoDataModel, ConsultarPedidoResponse>();
            cfg.CreateMap<PedidoRequestDto, PedidoDataModel>();
        });
        _mapper = new Mapper(config);

        // Configurar o repositório falso (mockado)
        _pedidoRepository = Substitute.For<IRepository<PedidoDataModel, Pedido>>();

        // Criar o serviço
        _pedidoService = new PedidoService(_pedidoRepository, _mapper);
    }

    // Inicializa o container do SQL Server e configura o banco
    public async Task InitializeAsync()
    {
        // Inicia o TestContainer
        await _sqlServerContainer.StartAsync();

        // Aqui você pode configurar o banco de dados, criar as tabelas, etc.
        // Caso esteja utilizando EF ou outra ferramenta, você pode garantir que o banco
        // tenha as tabelas criadas e inserir dados de teste, se necessário.
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        // Execute comandos SQL para criar tabelas ou inserir dados de teste, se necessário
        // Exemplo: Criar as tabelas ou qualquer preparação necessária no banco de dados

        // Feche a conexão após a configuração inicial
        await connection.CloseAsync();
    }

    // Finaliza o container após os testes
    public async Task DisposeAsync()
    {
        await _sqlServerContainer.StopAsync();
    }

    // Teste: Verificar se o Pedido já existe
    [Fact]
    public async Task CriarPedidoAsync_DeveRetornarErro_SePedidoJaExistir()
    {
        var pedidoRequest = new PedidoRequestDto { PedidoId = 1, Itens = new List<ItemPedidoRequestDto>() };
        _pedidoRepository.GetPedidoByIdWithItemsAsync(pedidoRequest.PedidoId).Returns(new ConsultarPedidoResponse());

        var resultado = await _pedidoService.CriarPedidoAsync(pedidoRequest);

        resultado.Mensagem.Should().Be("Pedido duplicado: Já existe um pedido com o mesmo ID.");
    }

    // Teste: Verificar se existe duplicidade de itens no pedido
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

    // Teste: Criar um novo pedido com sucesso
    [Fact]
    public async Task CriarPedidoAsync_DeveCriarPedido_ComSucesso()
    {
        var pedidoRequest = new PedidoRequestDto
        {
            PedidoId = 3,
            ClienteId = 123,
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

    // Teste: Consultar pedido existente
    [Fact]
    public async Task ConsultarPedidoAsync_DeveRetornarPedido_SeExistir()
    {
        var pedidoId = 1;
        var pedidoMock = new ConsultarPedidoResponse { PedidoId = pedidoId, Status = "Criado" };

        _pedidoRepository.GetPedidoByIdWithItemsAsync(pedidoId).Returns(pedidoMock);

        var resultado = await _pedidoService.ConsultarPedidoAsync(pedidoId);

        resultado.PedidoId.Should().Be(pedidoId);
    }

    // Teste: Consultar pedido inexistente
    [Fact]
    public async Task ConsultarPedidoAsync_DeveLancarExcecao_SeNaoEncontrado()
    {
        var pedidoId = 1;
        _pedidoRepository.GetPedidoByIdWithItemsAsync(pedidoId).Returns((ConsultarPedidoResponse)null);

        Func<Task> act = async () => await _pedidoService.ConsultarPedidoAsync(pedidoId);

        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Pedido não encontrado.");
    }
}