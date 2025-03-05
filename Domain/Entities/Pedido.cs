using Shared.Interfaces;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Pedido : IPedido
{
    public int PedidoId { get; set; }

    public int ClienteId { get; set; }

    public decimal? Imposto { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? DataPedido { get; set; }

    public virtual ICliente? Cliente { get; set; }

    public virtual ICollection<IItensPedido> ItensPedidos { get; set; } = new List<IItensPedido>();
}
