using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface ICliente
    {
        public int ClienteId { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public ICollection<IPedido> Pedidos { get; set; }
    }
}
