﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IProduto
    {
        public int ProdutoId { get; set; }

        public string Nome { get; set; }

        public decimal Preco { get; set; }

        public ICollection<IItensPedido> ItensPedidos { get; set; }
    }
}
