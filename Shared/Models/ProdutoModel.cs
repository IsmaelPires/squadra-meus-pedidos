namespace Shared.Models
{
    public class ProdutoModel
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public ICollection<ItensPedidoModel> ItensPedidos { get; set; }
    }
}
