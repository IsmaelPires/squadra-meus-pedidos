namespace Shared.Models
{
    public class ResultadoPedidoModel
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public PedidoModel Pedido { get; set; }
    }
}
