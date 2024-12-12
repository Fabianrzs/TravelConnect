using AutoMapper;

namespace TravelConnect.Infrastructure.Profiles;

public class HotelMappingProfile : Profile
{
    public HotelMappingProfile()
    {
        // Mapeo de CreateHotelRequest a Hotel
        CreateMap<CreateHotelRequest, Hotel>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src =>
                new Address(src.Street, src.City, src.State, src.ZipCode)))
            .ForMember(dest => dest.Rooms, opt => opt.Ignore()); // Inicialización manual en el dominio

        // Mapeo de Hotel a HotelResponse (si es necesario)
        CreateMap<Hotel, HotelResponse>()
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Address.State))
            .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Address.ZipCode));
    }
}
