using TravelConnect.Domain.Entities.Base;
using TravelConnect.Domain.Enums;

namespace TravelConnect.Domain.Entities;

public class Reservation : EntityBase
{
    public Guid RoomId { get; set; }
    public virtual Room Room { get; set; } = null!;
    public virtual List<Guest> Guests { get; set; } = [];
    public Guid EmergencyContactId { get; set; }
    public virtual EmergencyContact EmergencyContact { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalCost { get; set; }
    public ReservationStatus Status { get; set; }

    public void ValidateDates()
    {
        if (StartDate >= EndDate)
            throw new Exception("Start date must be before end date.");
    }
}
