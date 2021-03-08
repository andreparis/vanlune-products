using AutoMapper;
using Products.Domain.Entities;

namespace Products.Application.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Server, Variants>()
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Factor, opts => opts.MapFrom(src => 1.0))
                .ReverseMap();
        }
    }
}
