using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class PedidoDto
    {
        public int PedidoId { get; set; }
        public int ClienteId { get; set; }
        public decimal? Imposto { get; set; }
        public string Status { get; set; }
        public DateTime? DataPedido { get; set; }
        public ClienteDto Cliente { get; set; }
        public List<ItemPedidoDto> ItensPedidos { get; set; }
    }
}
