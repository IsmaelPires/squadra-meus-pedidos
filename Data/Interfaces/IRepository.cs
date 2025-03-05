using Shared.Dtos.Response;
using Data.Models;

namespace Data.Interfaces
{
    public interface IRepository<TModel, TDto> where TModel : class where TDto : class
    {
        Task<IEnumerable<TDto>> GetAll();
        Task<TDto> GetById(int id);
        Task Add(TDto entityDto);
        Task Update(TDto entityDto);
        Task Delete(int id);
        Task CriaPedidoAsync(PedidoDataModel pedidoDataModel);
        Task<ConsultarPedidoResponse> GetPedidoByIdWithItemsAsync(int id);
        Task<List<PedidoDataModel>> GetTodosPedidosAsync();
    }
}