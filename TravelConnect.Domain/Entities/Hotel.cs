using System.ComponentModel.DataAnnotations;
using TravelConnect.Domain.Entities.Base;
using TravelConnect.Domain.ValueObjects;

namespace TravelConnect.Domain.Entities;

public class Hotel : EntityBase
{
    public string Name { get; private set; } = string.Empty;
    public Address Address { get; private set; } = null!;
    public bool IsEnabled { get; private set; } = true;
    public List<Room> Rooms { get; private set; } = new();

    public void Initialize(string name, Address address, City city)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Hotel name cannot be empty.");

        Name = name;
        Address = address ?? throw new ValidationException("Address cannot be null.");
    }

    public void UpdateDetails(string name, Address address, City city)
    {
        Initialize(name, address, city);
    }

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
