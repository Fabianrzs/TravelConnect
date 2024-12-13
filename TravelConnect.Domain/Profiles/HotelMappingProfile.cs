using AutoMapper;
using TravelConnect.Commons.Models.Request;
using TravelConnect.Commons.Models.Response;
using TravelConnect.Domain.Entities;

namespace TravelConnect.Infrastructure.Profiles;

public class HotelMappingProfile : Profile
{
    public HotelMappingProfile()
    {
  
        CreateMap<HotelRequest, Hotel>()
            .ForMember(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street))
            .ForMember(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.Address.StateAdrees, opt => opt.MapFrom(src => src.StateAddress))
            .ForMember(dest => dest.Address.ZipCode, opt => opt.MapFrom(src => src.ZipCode))
            .ReverseMap() ;

        // Mapeo de Hotel a HotelResponse
        CreateMap<Hotel, HotelResponse>()
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Address.StateAdrees))
            .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Address.ZipCode))
            .ForMember(dest => dest.Rooms, opt => opt.Ignore()) // Manejar los Rooms según necesidad
            .ReverseMap();
    }
}
