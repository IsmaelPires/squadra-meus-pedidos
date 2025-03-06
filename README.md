# squadra-meus-pedidos


# Como Inicializar a API .NET 8 e Consumir os Métodos

Este README descreve como inicializar a API em .NET 8 e consumir os métodos de pedido, além de explicar como realizar testes unitários e de integração.

## Inicializando a API

### Requisitos
- .NET 8 SDK
- SQL Server

### Passos para Inicializar a API

1. **Clone o repositório**:
   ```bash
   git clone https://github.com/IsmaelPires/squadra-meus-pedidos.git
   cd <diretório-do-repositório>
   ```

2. **Restaure as dependências**:
   ```bash
   dotnet restore
   ```

3. **Inicialize o banco de dados** (conforme as instruções abaixo para criação do banco e tabelas).

4. **Execute a API**:
   ```bash
   dotnet run
   ```

   A API estará disponível em `https://localhost:5001` por padrão.

## Endpoints da API

### 1. Receber Pedido
- **Método**: POST
- **URL**: `api/pedidos/receberPedido`
- **Descrição**: Recebe um novo pedido.
- **Corpo da Requisição** (Exemplo):
  ```json
  {
    "pedidoId": 1,
    "itens": [
      {
        "produtoId": 1,
        "quantidade": 2,
        "valor": 39.98
      }
    ]
  }
  ```
- **Resposta**:
  - 201 (Created) se o pedido for criado com sucesso.
  - 400 (Bad Request) se houver erros.

### 2. Consultar Pedido
- **Método**: GET
- **URL**: `api/pedidos/consultarPedido/{id}`
- **Descrição**: Consulta um pedido pelo ID.
- **Resposta**:
  - 200 (OK) com os dados do pedido.
  - 404 (Not Found) se o pedido não for encontrado.

### 3. Listar Pedidos por Status
- **Método**: GET
- **URL**: `api/pedidos/listarPedidosPorStatus?status={status}`
- **Descrição**: Lista pedidos filtrando por status.
- **Resposta**:
  - 200 (OK) com a lista de pedidos.
  - 404 (Not Found) se não houver pedidos com o status fornecido.

## Testes Unitários

Os testes unitários são realizados usando o framework `xUnit`.

### Executando os Testes Unitários

1. **Restaurar dependências**:
   ```bash
   dotnet restore
   ```

2. **Executar os testes**:
   ```bash
   dotnet test
   ```

### Exemplos de Testes Unitários

Aqui estão alguns exemplos de testes unitários implementados para o serviço `PedidoService`:

#### Teste de Erro de Pedido Duplicado
```csharp
[Fact]
public async Task CriarPedidoAsync_DeveRetornarErro_SePedidoJaExistir()
{
    var pedidoRequest = new PedidoRequestDto { PedidoId = 1, Itens = new List<ItemPedidoRequestDto>() };
    _pedidoRepository.GetPedidoByIdWithItemsAsync(pedidoRequest.PedidoId).Returns(new ConsultarPedidoResponse());

    var resultado = await _pedidoService.CriarPedidoAsync(pedidoRequest);

    resultado.Mensagem.Should().Be("Pedido duplicado: Já existe um pedido com o mesmo ID.");
}
```

#### Teste de Criação de Pedido com Sucesso
```csharp
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
```

## Testes de Integração

Os testes de integração são realizados usando o `Testcontainers` para iniciar um container SQL Server e realizar operações no banco de dados.

### Inicializando os Testes de Integração

1. **Restaurar dependências**:
   ```bash
   dotnet restore
   ```

2. **Executar os testes**:
   ```bash
   dotnet test
   ```

### Exemplo de Teste de Integração

Aqui está um exemplo de teste de integração para criar um pedido:

```csharp
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

    var resultado = await _pedidoService.CriarPedidoAsync(pedidoRequest);

    resultado.Status.Should().Be("Criado");
    resultado.Id.Should().Be(pedidoRequest.PedidoId);
}
```

## Scripts para Banco de Dados

### Criar Banco de Dados e Tabelas

Utilize o seguinte script SQL para criar o banco de dados e as tabelas necessárias:

```sql
-- Criação do banco de dados
CREATE DATABASE Pedidos;

-- Conectar ao banco de dados
USE Pedidos;

-- Criando a tabela Clientes
CREATE TABLE Clientes (
    ClienteId INT PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL
);

-- Criando a tabela Produtos
CREATE TABLE Produtos (
    ProdutoId INT PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    Preco DECIMAL(10, 2) NOT NULL
);

-- Criando a tabela Pedidos
CREATE TABLE Pedidos (
    PedidoId INT PRIMARY KEY,
    ClienteId INT,
    Imposto DECIMAL(10, 2),
    Status NVARCHAR(50) NOT NULL,
    DataPedido DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Pedidos_Clientes FOREIGN KEY (ClienteId) REFERENCES Clientes (ClienteId)
);

-- Criando a tabela ItensPedidos
CREATE TABLE ItensPedidos (
    ItemPedidoId INT PRIMARY KEY IDENTITY(1,1),
    PedidoId INT,
    ProdutoId INT,
    Quantidade INT NOT NULL,
    Valor DECIMAL(10, 2) NOT NULL,
    CONSTRAINT FK_ItensPedidos_Pedidos FOREIGN KEY (PedidoId) REFERENCES Pedidos (PedidoId),
    CONSTRAINT FK_ItensPedidos_Produtos FOREIGN KEY (ProdutoId) REFERENCES Produtos (ProdutoId)
);
```

### Inserir Dados de Exemplo

Utilize os seguintes scripts para inserir dados nas tabelas:

```sql
-- Inserindo dados na tabela Clientes
INSERT INTO Clientes (ClienteId, Nome, Email) VALUES
(1, 'João Silva', 'joao.silva@example.com'),
(2, 'Maria Oliveira', 'maria.oliveira@example.com');

-- Inserindo dados na tabela Produtos
INSERT INTO Produtos (ProdutoId, Nome, Preco) VALUES
(1, 'Produto A', 19.99),
(2, 'Produto B', 29.99);

-- Inserindo dados na tabela Pedidos
INSERT INTO Pedidos (PedidoId, ClienteId, Imposto, Status) VALUES
(1, 1, 5.00, 'Processando'),
(2, 2, 10.00, 'Enviado');

-- Inserindo dados na tabela ItensPedidos
INSERT INTO ItensPedidos (PedidoId, ProdutoId, Quantidade, Valor) VALUES
(1, 1, 2, 39.98),
(2, 2, 1, 29.99);
```

## Conexão com o Banco de Dados

A connection string para o banco de dados usada localmente foi a seguinte:

```json
"PedidosConnection": "Server=ISMANITRO5\SQLEXPRESS;Database=Pedidos;User Id=pedidos;Password=pedidos@123;TrustServerCertificate=True;"
```
Alterar o servidor conforme desejado.
