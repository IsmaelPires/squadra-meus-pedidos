using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IPedido
    {
        public int PedidoId { get; set; }

        public int ClienteId { get; set; }

        public decimal? Imposto { get; set; }

        public string Status { get; set; }

        public DateTime? DataPedido { get; set; }

        public ICliente? Cliente { get; set; }

        public ICollection<IItensPedido> ItensPedidos { get; set; }
    }
}
