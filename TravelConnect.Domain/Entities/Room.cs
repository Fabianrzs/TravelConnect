
using TravelConnect.Domain.Entities.Base;
using TravelConnect.Domain.Enums;

namespace TravelConnect.Domain.Entities;

public class Room : EntityBase
{
    public Guid HotelId { get; set; }
    public RoomType RoomType { get; set; }
    public decimal BaseCost { get; set; }
    public decimal Taxes { get; set; }
    public bool IsEnabled { get; set; }
    public string Location { get; set; } = string.Empty;
    public List<Reservation> Reservations { get; set; } = new();

    public decimal CalculateTotalCost() => BaseCost + Taxes;

    public bool HasActiveReservations()
    {
        var currentDate = DateTime.UtcNow;
        return Reservations.Any(reservation =>
            reservation.Status == ReservationStatus.Confirmed && // Enum utilizado aquí
            reservation.StartDate <= currentDate &&
            reservation.EndDate >= currentDate);
    }
}
