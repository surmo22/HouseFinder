using AutoMapper;
using HouseFinderBackEnd.Data.Buildings;
using HouseFinderBackEnd.Data.Models;

namespace HouseFinderBackEnd.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Property, PropertyModel>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.Name));

            CreateMap<PropertyModel, Property>()
                .ForMember(dest => dest.City, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Watchers, opt => opt.Ignore());
        }
    }
}