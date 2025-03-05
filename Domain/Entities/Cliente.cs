using Shared.Interfaces;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Cliente : ICliente
{
    public int ClienteId { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<IPedido> Pedidos { get; set; } = new List<IPedido>();
}
