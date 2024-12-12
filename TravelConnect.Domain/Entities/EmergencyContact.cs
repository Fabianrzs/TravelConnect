using TravelConnect.Domain.Entities.Base;

namespace TravelConnect.Domain.Entities;

public class EmergencyContact : EntityBase
{
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}
