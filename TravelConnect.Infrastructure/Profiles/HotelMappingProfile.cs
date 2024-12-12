using AutoMapper;
using TravelConnect.Commons.Models.Request;
using TravelConnect.Commons.Models.Response;
using TravelConnect.Domain.Entities;
using TravelConnect.Domain.ValueObjects;

namespace TravelConnect.Commons.Profiles;

public class HotelMappingProfile : Profile
{
    public HotelMappingProfile()
    {
        CreateMap<HotelRequest, Hotel>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src =>
                new Address(src.Street, src.City, src.State, src.ZipCode)))
            .ForMember(dest => dest.Rooms, opt => opt.Ignore()); 

        CreateMap<Hotel, HotelResponse>()
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Address.State))
            .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Address.ZipCode));
    }
}
