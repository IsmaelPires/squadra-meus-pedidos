using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IItensPedido
    {
        public int ItemPedidoId { get; set; }

        public int PedidoId { get; set; }

        public int ProdutoId { get; set; }

        public int Quantidade { get; set; }

        public decimal Valor { get; set; }

        public IPedido? Pedido { get; set; }

        public IProduto? Produto { get; set; }
    }
}
