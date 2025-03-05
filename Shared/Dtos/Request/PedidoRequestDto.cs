using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Request
{
    public class PedidoRequestDto
    {
        public int PedidoId { get; set; }
        public int ClienteId { get; set; }
        public List<ItemPedidoRequestDto> Itens { get; set; }
    }
}
