using Shared.Interfaces;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Produto : IProduto
{
    public int ProdutoId { get; set; }

    public string Nome { get; set; } = null!;

    public decimal Preco { get; set; }

    public virtual ICollection<IItensPedido> ItensPedidos { get; set; } = new List<IItensPedido>();
}
