﻿using AutoMapper;
using TravelConnect.Commons.Models.Request;
using TravelConnect.Commons.Models.Response;
using TravelConnect.Domain.Entities;
using TravelConnect.Domain.Enums;
using TravelConnect.Domain.Extensions;

namespace TravelConnect.Infrastructure.Profiles;

public class RoomMappingProfile : Profile
{
    public RoomMappingProfile()
    {
        CreateMap<RoomRequest, Room>()
                .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => Enum.Parse<RoomType>(src.RoomType, true)))
                .ReverseMap();                

        CreateMap<Room, RoomResponse>()
            .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.RoomType.ToFriendlyString()))
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name))
            .ForMember(dest => dest.HasActiveReservations, opt => opt.MapFrom(src => src.HasActiveReservations()));
    }
}
