using TravelConnect.Domain.Entities.Base;

namespace TravelConnect.Domain.Entities;

public class Hotel : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public List<Room> Rooms { get; set; } = new();

    public void Disable()
    {
        if (Rooms.Exists(r => r.HasActiveReservations()))
            throw new Exception("Cannot disable a hotel with active reservations.");
        IsEnabled = false;
    }
}