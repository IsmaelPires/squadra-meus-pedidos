using AutoMapper;
using Data.Models;
using Shared.Dtos.Response;

namespace Data.Mapper
{
    public class DataMappingProfile : Profile
    {
        public DataMappingProfile()
        {
            // Mapeamento entre DataModel e DTO
            CreateMap<PedidoDataModel, ConsultarPedidoResponse>()
                .ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.ItensPedidos));

            CreateMap<ItensPedidoDataModel, ItemPedidoResponse>()
                .ForMember(dest => dest.ProdutoId, opt => opt.MapFrom(src => src.Produto.ProdutoId));
        }
    }
}