using AutoMapper.Configuration.Annotations;
using TravelConnect.Domain.Entities.Base;
using TravelConnect.Domain.Enums;

namespace TravelConnect.Domain.Entities;

public class Guest : EntityBase
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public Guid ReservationId { get; set; }
    [Ignore]
    public virtual Reservation Reservation { get; set; } = null!;
}
