using AutoMapper;
using Shared.Dtos.Response;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using Data.Models;

namespace Data.Repository
{
    public class Repository<TModel, TDto> : IRepository<TModel, TDto>
        where TModel : class where TDto : class
    {
        private readonly PedidosContext _context;
        private readonly IMapper _mapper;
        private readonly DbSet<TModel> _dbSet;

        public Repository(PedidosContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _dbSet = _context.Set<TModel>();
        }

        #region Métodos Genéricos
        public async Task<IEnumerable<TDto>> GetAll()
        {
            var entities = await _dbSet.ToListAsync();
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

        public async Task<TDto> GetById(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return _mapper.Map<TDto>(entity);
        }

        public async Task Add(TDto entityDto)
        {
            var entity = _mapper.Map<TModel>(entityDto);
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(TDto entityDto)
        {
            var entity = _mapper.Map<TModel>(entityDto);
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        #endregion

        #region Métodos Customizados
        public async Task<ConsultarPedidoResponse> GetPedidoByIdWithItemsAsync(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.ItensPedidos)
                .ThenInclude(ip => ip.Produto)
                .FirstOrDefaultAsync(p => p.PedidoId == id);

            return _mapper.Map<ConsultarPedidoResponse>(pedido);
        }

        public async Task<List<PedidoDataModel>> GetTodosPedidosAsync()
        {
            return await _context.Pedidos
                .Include(p => p.ItensPedidos) // Inclui os itens do pedido
                .Include(p => p.Cliente) // Inclui o cliente, se necessário
                .ToListAsync();
        }

        public async Task CriaPedidoAsync(PedidoDataModel pedidoDataModel)
        {
            var entity = _mapper.Map<TModel>(pedidoDataModel);
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}