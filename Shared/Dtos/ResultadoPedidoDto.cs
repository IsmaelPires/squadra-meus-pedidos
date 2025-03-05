namespace Shared.Dtos
{
    public class ResultadoPedidoDto
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public PedidoDto Pedido { get; set; }
    }
}
