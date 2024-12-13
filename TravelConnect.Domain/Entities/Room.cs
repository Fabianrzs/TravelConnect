using TravelConnect.Domain.Enums;
using TravelConnect.Domain.Entities.Base;
using TravelConnect.Domain.Exceptions;

namespace TravelConnect.Domain.Entities
{
    public class Room : EntityBase
    {
        public Guid HotelId { get; set; }
        public virtual Hotel Hotel { get; set; } = null!; 
        public RoomType RoomType { get; set; }
        public decimal BaseCost { get; set; }
        public decimal Taxes { get; set; }
        public bool IsEnabled { get; set; } = true;
        public string Location { get; set; } = string.Empty;
        public virtual List<Reservation> Reservations { get; set; } = [];
        public void Initialize(Guid hotelId, string roomType, decimal baseCost, decimal taxes, string location)
        {
            if (baseCost < 0)
                throw new ArgumentException("Base cost cannot be negative.", nameof(baseCost));
            if (taxes < 0)
                throw new ArgumentException("Taxes cannot be negative.", nameof(taxes));
            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("Location cannot be empty.", nameof(location));

            HotelId = hotelId;
            RoomType = Enum.Parse<RoomType>(roomType, true);
            BaseCost = baseCost;
            Taxes = taxes;
            Location = location;
        }

        public void UpdateDetails(decimal baseCost, decimal taxes, string roomType, string location)
        {
            Initialize(HotelId, roomType, baseCost, taxes, location);
        }
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
