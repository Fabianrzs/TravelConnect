using AutoMapper;
using TravelConnect.Commons.Models.Response;
using TravelConnect.Domain.Entities;
using TravelConnect.Domain.Exceptions;
using TravelConnect.Domain.Ports.Persistence;
using TravelConnect.Domain.Services;

namespace TravelConnect.Application.Services;
[DomainService]
public class ReservationService(
    IRepository<Reservation> reservationRepository, IMapper mapper)
{
    public async Task<IEnumerable<ReservationResponse>> GetReservationsByRoomAsync(Guid roomId)

    {
        var response = await reservationRepository.FindAsync(r => r.RoomId == roomId,
            r => r.Guests, r => r.EmergencyContact);

        return mapper.Map< IEnumerable<ReservationResponse>>(response);
    }

    /* El sistema deberá permitir ver el detalle de cada una de las reservas realizadas */
    public async Task<ReservationResponse> GetReservationDetailsAsync(Guid reservationId)
    {
        var reservation = await reservationRepository.GetByIdAsync(reservationId,
            r => r.Guests, r => r.EmergencyContact) ?? throw new NotFoundException("Reservation", reservationId);
        return mapper.Map<ReservationResponse>(reservation);

    }

    /* El sistema deberá listar cada una de las reservas realizadas en mis hoteles */
    public async Task<IEnumerable<ReservationResponse>> GetReservationsByHotelAsync(Guid hotelId)
    {
        var response = await reservationRepository.FindAsync(r => r.Room.HotelId == hotelId, r => r.Room, r => r.Room.Hotel,
            r => r.Guests, r => r.EmergencyContact);

        return mapper.Map<IEnumerable<ReservationResponse>>(response);
    }
}
