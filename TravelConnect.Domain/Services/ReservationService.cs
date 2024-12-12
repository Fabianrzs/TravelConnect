using TravelConnect.Domain.Entities;
using TravelConnect.Domain.Enums;
using TravelConnect.Domain.Exceptions;
using TravelConnect.Domain.Ports;

namespace TravelConnect.Application.Services
{
    public class ReservationService(
        IRepository<Reservation> reservationRepository)
    {
        public async Task<IEnumerable<Reservation>> GetReservationsByRoomAsync(Guid roomId)
        {
            return await reservationRepository.FindAsync(r => r.RoomId == roomId);
        }

        /* El sistema deberá permitir ver el detalle de cada una de las reservas realizadas */
        public async Task<Reservation> GetReservationDetailsAsync(Guid reservationId)
        {
            var reservation = await reservationRepository.GetByIdAsync(reservationId);
            return reservation ?? throw new NotFoundException("Reservation", reservationId);
        }

        /* El sistema deberá listar cada una de las reservas realizadas en mis hoteles */
        public async Task<IEnumerable<Reservation>> GetReservationsByHotelAsync(Guid hotelId)
        {
            return await reservationRepository.FindAsync(r => r.HotelId == hotelId);
        }
    }
}
