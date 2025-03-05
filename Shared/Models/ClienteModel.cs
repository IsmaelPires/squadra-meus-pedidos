namespace Shared.Models
{
    public class ClienteModel
    {
        public int ClienteId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public ICollection<PedidoModel> Pedidos { get; set; }
    }
}
