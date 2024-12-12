namespace TravelConnect.Domain.Entities.Base;

public class DomainEntity
{
    public DateTime CreatedOn { get; set; }
    public DateTime LastModifiedOn { get; set; }
    public bool State { get; set; } = true;
}
