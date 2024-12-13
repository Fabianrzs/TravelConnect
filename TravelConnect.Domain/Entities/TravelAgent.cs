using TravelConnect.Domain.Entities.Base;

namespace TravelConnect.Domain.Entities;

public class TravelAgent : EntityBase
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public virtual List<Hotel> HotelsManaged { get; set; } = [];
}
