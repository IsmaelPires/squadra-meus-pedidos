using Shared.Interfaces;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class ItensPedido : IItensPedido
{
    public int ItemPedidoId { get; set; }

    public int PedidoId { get; set; }

    public int ProdutoId { get; set; }

    public int Quantidade { get; set; }

    public decimal Valor { get; set; }

    public virtual IPedido? Pedido { get; set; }

    public virtual IProduto? Produto { get; set; }
}
