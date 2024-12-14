using System.ComponentModel.DataAnnotations;
using TravelConnect.Domain.Entities.Base;
using TravelConnect.Domain.ValueObjects;

namespace TravelConnect.Domain.Entities;

public class Hotel : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public virtual Address Address { get; set; } = new();
    public bool IsEnabled { get; set; } = true;
    public virtual List<Room> Rooms { get; set; } = [];

    public void Disable()
    {
        if (Rooms.Any(r => r.HasActiveReservations()))
            throw new ValidationException("Cannot disable a hotel with active reservations.");
        IsEnabled = false;
    }

    public void Enable()
    {
        IsEnabled = true;
    }
}
