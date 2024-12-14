using AutoMapper;
using TravelConnect.Commons.Models.Request;
using TravelConnect.Commons.Models.Response;
using TravelConnect.Domain.Entities;
using TravelConnect.Domain.Enums;

namespace TravelConnect.Infrastructure.Profiles;

public class ReservationProfile: Profile
{
    public ReservationProfile()
    {
        CreateMap<ReservationRequest, Reservation>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.CheckIn))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.CheckOut))
                //.ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => Enum.Parse<RoomType>(src.RoomType, true)))
                .ReverseMap();

        CreateMap<EmergencyContactRequest, EmergencyContact>()
                .ReverseMap();

        CreateMap<GuestRequest,Guest>()
                .ReverseMap();

        CreateMap<ReservationResponse, Reservation>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.CheckIn))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.CheckOut))
            .ReverseMap();
    }
}
