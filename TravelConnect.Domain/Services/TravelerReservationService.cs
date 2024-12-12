using TravelConnect.Domain.Entities;
using TravelConnect.Domain.Enums;
using TravelConnect.Domain.Exceptions;
using TravelConnect.Domain.Ports.Notifications;
using TravelConnect.Domain.Ports.Persistence;

namespace TravelConnect.Application.Services
{
    public class TravelerReservationService(
        IRepository<Hotel> hotelRepository,
        IRepository<Room> roomRepository,
        IRepository<Reservation> reservationRepository,
        INotificationService notificationService,
        IUnitOfWork unitOfWork)
    {

        /* 
        El sistema me deberá dar la opción de buscar por:
        fecha de entrada al alojamiento, fecha de salida del alojamiento,
        cantidad de personas que se alojarán y ciudad de destino.
        */
        public async Task<IEnumerable<Hotel>> SearchHotelsAsync(DateTime checkIn, DateTime checkOut, int numberOfGuests, string city)
        {
            if (checkIn >= checkOut)
                throw new BusinessRuleViolationException("Check-in date must be before check-out date.");

            return await hotelRepository.FindAsync(h =>
                h.Address.City.Equals(city, StringComparison.OrdinalIgnoreCase) &&
                h.Rooms.Any(r => r.IsEnabled && r.IsAvailable(checkIn, checkOut, numberOfGuests)));
        }

        /*
        El sistema me deberá permitir elegir una habitación del hotel de mi preferencia.
        */
        public async Task<Room> SelectRoomAsync(Guid hotelId, Guid roomId)
        {
            var room = await roomRepository.GetByIdAsync(roomId);

            if (room == null || room.HotelId != hotelId || !room.IsEnabled)
                throw new BusinessRuleViolationException("The selected room is not available or does not belong to the specified hotel.");

            return room;
        }

        /*
        El sistema me deberá desplegar un formulario de reserva para ingresar los datos de los huéspedes.
        */
        public static Reservation CreateReservationDraft(Guid roomId, DateTime checkIn, DateTime checkOut, List<Guest> guests, EmergencyContact emergencyContact)
        {
            if (checkIn >= checkOut)
                throw new BusinessRuleViolationException("Check-in date must be before check-out date.");

            if (!guests.Any())
                throw new BusinessRuleViolationException("At least one guest must be provided.");

            return new Reservation
            {
                RoomId = roomId,
                StartDate = checkIn,
                EndDate = checkOut,
                Guests = guests,
                EmergencyContact = emergencyContact,
                Status = ReservationStatus.Pending
            };
        }

        /*
        El sistema deberá permitir realizar la reserva de la habitación.
        */
        public async Task CompleteReservationAsync(Reservation reservation)
        {
            ArgumentNullException.ThrowIfNull(reservation);

            var room = await roomRepository.GetByIdAsync(reservation.RoomId);

            if (room == null || !room.IsEnabled || !room.IsAvailable(reservation.StartDate, reservation.EndDate, reservation.Guests.Count))
                throw new BusinessRuleViolationException("The room is not available for the selected dates.");

            reservation.Status = ReservationStatus.Confirmed;
            await reservationRepository.AddAsync(reservation);
            await unitOfWork.CommitAsync();

            /*
            El sistema me deberá notificar la reserva por medio de correo electrónico.
            */
            var guestEmails = reservation.Guests.Select(g => g.Email.ToString()).ToList();
            foreach (var email in guestEmails)
            {
                await notificationService.SendEmailAsync(
                    email,
                    "Reservation Confirmation",
                    $"Your reservation at room {room!.RoomType} has been confirmed for the dates {reservation.StartDate:dd/MM/yyyy} to {reservation.EndDate:dd/MM/yyyy}."
                );
            }
        }

        public async Task CancelReservationAsync(Guid reservationId)
        {
            var reservation = await reservationRepository.GetByIdAsync(reservationId) ?? throw new NotFoundException("Reservation", reservationId);
            if (reservation.Status == ReservationStatus.Canceled)
                throw new BusinessRuleViolationException("The reservation has already been canceled.");

            reservation.Status = ReservationStatus.Canceled;
            await reservationRepository.UpdateAsync(reservation);
            await unitOfWork.CommitAsync();

            var guestEmails = reservation.Guests.Select(g => g.Email.ToString()).ToList();
            foreach (var email in guestEmails)
            {
                await notificationService.SendEmailAsync(
                    email,
                    "Reservation Canceled",
                    $"Your reservation for room {reservation.RoomId} from {reservation.StartDate:dd/MM/yyyy} to {reservation.EndDate:dd/MM/yyyy} has been canceled. We apologize for the inconvenience."
                );
            }
        }
    }
}
