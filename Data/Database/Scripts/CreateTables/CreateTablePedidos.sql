-- Tabela Pedidos
CREATE TABLE Pedidos (
    PedidoId INT PRIMARY KEY,
    ClienteId INT,
    Imposto DECIMAL(10, 2),
    Status NVARCHAR(50) NOT NULL,
    DataPedido DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Pedidos_Clientes FOREIGN KEY (ClienteId) REFERENCES Clientes (ClienteId)
);