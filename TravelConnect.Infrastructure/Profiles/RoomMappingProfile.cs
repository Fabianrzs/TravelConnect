using AutoMapper;
using TravelConnect.Commons.Models.Request;
using TravelConnect.Commons.Models.Response;
using TravelConnect.Domain.Entities;
using TravelConnect.Domain.Enums;

namespace TravelConnect.Infrastructure.Profiles;

public class RoomMappingProfile : Profile
{
    public RoomMappingProfile()
    {
        CreateMap<RoomRequest, Room>()
                .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => Enum.Parse<RoomType>(src.RoomType, true)))
                .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IsEnabled))
                .ForMember(dest => dest.Reservations, opt => opt.Ignore()) 

                
                .AfterMap((src, dest) => dest.Initialize(src.HotelId, Enum.Parse<RoomType>(src.RoomType, true), src.BaseCost, src.Taxes, src.Location));

        CreateMap<Room, RoomResponse>()
            .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.RoomType.ToString()))
            .ForMember(dest => dest.HasActiveReservations, opt => opt.MapFrom(src => src.HasActiveReservations()));
    }
}
