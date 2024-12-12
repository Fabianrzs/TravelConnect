namespace TravelConnect.Domain.Entities.Base;

public class DomainEntity
{
    public DateTime CreatedOn { get; set; }
    //public DateTime CreatedUser { get; set; }
    public DateTime LastModifiedOn { get; set; }
    //public DateTime LastModifiedUser { get; set; }
    public bool State { get; set; } = true;
}
