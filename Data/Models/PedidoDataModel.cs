using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class PedidoDataModel
    {
        public int PedidoId { get; set; }

        public int ClienteId { get; set; }

        public decimal? Imposto { get; set; }

        public string Status { get; set; } = null!;

        public DateTime? DataPedido { get; set; }

        public ClienteDataModel? Cliente { get; set; }

        public ICollection<ItensPedidoDataModel> ItensPedidos { get; set; } = new List<ItensPedidoDataModel>();
    }
}
