-- Tabela Produtos
CREATE TABLE Produtos (
    ProdutoId INT PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    Preco DECIMAL(10, 2) NOT NULL
);