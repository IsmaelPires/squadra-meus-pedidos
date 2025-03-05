using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class ItemPedidoDto
    {
        public int ItemPedidoId { get; set; }
        public int PedidoId { get; set; }
        public int ProdutoId { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
    }
}
