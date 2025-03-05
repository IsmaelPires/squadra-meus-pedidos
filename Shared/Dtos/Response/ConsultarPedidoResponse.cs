using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Response
{
    public class ConsultarPedidoResponse
    {
        public int PedidoId { get; set; }
        public int ClienteId { get; set; }
        public decimal? Imposto { get; set; }
        public List<ItemPedidoResponse> Itens { get; set; }
        public string Status { get; set; }
        public DateTime? DataPedido { get; set; }
    }

    public class ItemPedidoResponse
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
    }
}
