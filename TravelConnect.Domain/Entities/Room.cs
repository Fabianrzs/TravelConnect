using TravelConnect.Domain.Enums;
using TravelConnect.Domain.Entities.Base;
using TravelConnect.Domain.Exceptions;

namespace TravelConnect.Domain.Entities
{
    public class Room : EntityBase
    {
        public Guid HotelId { get; private set; }
        public virtual Hotel Hotel { get; set; } = new();
        public RoomType RoomType { get; private set; }
        public decimal BaseCost { get; private set; }
        public decimal Taxes { get; private set; }
        public bool IsEnabled { get; private set; } = true;
        public string Location { get; private set; } = string.Empty;
        public virtual List<Reservation> Reservations { get; private set; } = [];

        public void Disable()
        {
            if (HasActiveReservations())
                throw new ValidationException("Cannot disable a room with active reservations.");
            IsEnabled = false;
        }

        public void Enable()
        {
            IsEnabled = true;
        }

        public bool HasActiveReservations()
        {
            var currentDate = DateTime.UtcNow;
            return Reservations.Any(r =>
                r.Status == ReservationStatus.Confirmed &&
                r.StartDate <= currentDate &&
                r.EndDate >= currentDate);
        }

        public bool IsAvailable(DateTime checkIn, DateTime checkOut, int numberOfGuests)
        {
            if (!IsEnabled)
                return false;

            return !Reservations.Any(r =>
                r.Status == ReservationStatus.Confirmed &&
                r.StartDate < checkOut && 
                r.EndDate > checkIn); 
        }
    }
}
