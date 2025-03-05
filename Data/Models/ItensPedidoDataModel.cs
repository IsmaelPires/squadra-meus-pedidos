using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ItensPedidoDataModel
    {
        public int ItemPedidoId { get; set; }

        public int PedidoId { get; set; }

        public int ProdutoId { get; set; }

        public int Quantidade { get; set; }

        public decimal Valor { get; set; }

        public virtual PedidoDataModel? Pedido { get; set; }

        public virtual ProdutoDataModel? Produto { get; set; }
    }
}