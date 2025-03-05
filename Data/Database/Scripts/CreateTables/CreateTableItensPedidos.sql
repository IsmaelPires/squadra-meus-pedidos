-- Tabela ItensPedidos
CREATE TABLE ItensPedidos (
    ItemPedidoId INT PRIMARY KEY IDENTITY(1,1),
    PedidoId INT,
    ProdutoId INT,
    Quantidade INT NOT NULL,
    Valor DECIMAL(10, 2) NOT NULL,
    CONSTRAINT FK_ItensPedidos_Pedidos FOREIGN KEY (PedidoId) REFERENCES Pedidos (PedidoId),
    CONSTRAINT FK_ItensPedidos_Produtos FOREIGN KEY (ProdutoId) REFERENCES Produtos (ProdutoId)
);