using Shared.Dtos.Request;
using Shared.Dtos.Response;

namespace Domain.Interfaces
{
    public interface IPedidoService
    {
        Task<ReceberPedidoResponse> CriarPedidoAsync(PedidoRequestDto pedido);
        Task<ConsultarPedidoResponse> ConsultarPedidoAsync(int id);
        Task<List<ConsultarPedidoResponse>> ListarPedidosPorStatusAsync(string status);
    }
}