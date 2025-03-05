using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class PedidoModel
    {
        public int PedidoId { get; set; }
        public int ClienteId { get; set; }
        public decimal Imposto { get; set; }
        public string Status { get; set; }
        public List<ItensPedidoModel> Itens { get; set; }
    }
}
