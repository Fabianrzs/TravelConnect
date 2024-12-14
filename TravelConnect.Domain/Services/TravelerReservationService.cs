using AutoMapper;
using System.Threading;
using TravelConnect.Commons.Models.Request;
using TravelConnect.Commons.Models.Response;
using TravelConnect.Domain.Entities;
using TravelConnect.Domain.Enums;
using TravelConnect.Domain.Exceptions;
using TravelConnect.Domain.Extensions;
using TravelConnect.Domain.Ports.Notifications;
using TravelConnect.Domain.Ports.Persistence;
using TravelConnect.Domain.Services;

namespace TravelConnect.Application.Services;
[DomainService]
public class TravelerReservationService(
    IRepository<Hotel> hotelRepository,
    IRepository<Room> roomRepository,
    IRepository<Reservation> reservationRepository,
    INotificationService notificationService,
    IUnitOfWork unitOfWork, IMapper mapper)
{

    /* 
    El sistema me deberá dar la opción de buscar por:
    fecha de entrada al alojamiento, fecha de salida del alojamiento,
    cantidad de personas que se alojarán y ciudad de destino.
    */
    public async Task<IEnumerable<HotelResponse>> SearchHotelsAsync(DateTime checkIn, DateTime checkOut, int numberOfGuests, string city)
    {
        if (checkIn >= checkOut)
            throw new BusinessRuleViolationException("Check-in date must be before check-out date.");

        var response =  await hotelRepository.FindAsync(h =>
            h.Address.City.Equals(city),h => h.Address, h => h.Rooms);

        response.Select(h => h.Rooms.Any(r => r.IsEnabled && r.IsAvailable(checkIn, checkOut, numberOfGuests)));

        return mapper.Map<IEnumerable<HotelResponse>>(response);
    }

    /*
    El sistema me deberá permitir elegir una habitación del hotel de mi preferencia.
    */
    public async Task<RoomResponse> SelectRoomAsync(Guid hotelId, Guid roomId)
    {
        var room = await roomRepository.GetByIdAsync(roomId, r => r.Hotel);

        if (room == null || room.HotelId != hotelId || !room.IsEnabled)
            throw new BusinessRuleViolationException("The selected room is not available or does not belong to the specified hotel.");

        return mapper.Map<RoomResponse>(room);
    }

    /*
    El sistema me deberá desplegar un formulario de reserva para ingresar los datos de los huéspedes.
    */
    public static Reservation CreateReservationDraft(Guid roomId, DateTime checkIn, DateTime checkOut, List<Guest> guests, EmergencyContact emergencyContact)
    {
        if (checkIn >= checkOut)
            throw new BusinessRuleViolationException("Check-in date must be before check-out date.");

        if (guests.Count == 0)
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
    public async Task CompleteReservationAsync(ReservationRequest reservationRequest)
    {
        ArgumentNullException.ThrowIfNull(reservationRequest);

        reservationRequest.Id = Guid.NewGuid();
        var reservation = mapper.Map<Reservation>(reservationRequest);
        reservation.EmergencyContact.Id = Guid.NewGuid();
        reservation.EmergencyContactId = reservation.EmergencyContact.Id;

        var room = await roomRepository.GetByIdAsync(reservation.RoomId, r => r.Hotel)
                   ?? throw new BusinessRuleViolationException("The room was not found.");
        if (room.Hotel == null)
            throw new BusinessRuleViolationException("The room is not associated with a valid hotel.");
        if (!room.IsEnabled || !room.IsAvailable(reservation.StartDate, reservation.EndDate, reservation.Guests.Count))
            throw new BusinessRuleViolationException("The room is not available for the selected dates.");

        reservation.RoomId = room.Id;
        reservation.TotalCost = room.CalculateCost(reservation.StartDate, reservation.EndDate);
        reservation.Status = ReservationStatus.Confirmed;

        foreach (var guest in reservation.Guests)
        {
            guest.Id = Guid.NewGuid();
            guest.ReservationId = reservation.Id;
        }

        if (reservation.Guests.Count == 0)
            throw new ValidationException("At least one guest is required.");

        await reservationRepository.AddAsync(reservation);
        await unitOfWork.CommitAsync();


        foreach (var email in reservation.Guests.Select(g => g.Email))
        {
            await notificationService.SendEmailAsync(
                email,
                $"Reservation Confirmation at {room.Hotel.Name}",
                $"Your reservation for {room.RoomType.ToFriendlyString()} is confirmed from {reservation.StartDate:dd/MM/yyyy} to {reservation.EndDate:dd/MM/yyyy}."
            );
        }
    }

    public async Task CancelReservationAsync(Guid reservationId)
    {
        var reservation = await reservationRepository.GetByIdAsync(reservationId, r => r.Guests, r => r.Room.Hotel) ?? throw new NotFoundException("Reservation", reservationId);
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
                $"Your reservation at Hotel {reservation.Room.Hotel.Name} " +
                $"from {reservation.StartDate:dd/MM/yyyy} " +
                $"to {reservation.EndDate:dd/MM/yyyy} " +
                $"has been canceled. We apologize for the inconvenience."
            );
        }
    }
}
