using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ClienteDataModel
    {
        public int ClienteId { get; set; }

        public string Nome { get; set; } = null!;

        public string Email { get; set; } = null!;

        public ICollection<PedidoDataModel> Pedidos { get; set; } = new List<PedidoDataModel>();
    }
}
