using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ProdutoDataModel
    {
        public int ProdutoId { get; set; }

        public string Nome { get; set; } = null!;

        public decimal Preco { get; set; }

        public virtual ICollection<ItensPedidoDataModel> ItensPedidos { get; set; } = new List<ItensPedidoDataModel>();
    }
}
