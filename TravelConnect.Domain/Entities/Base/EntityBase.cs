using System.ComponentModel.DataAnnotations;

namespace TravelConnect.Domain.Entities.Base;

public class EntityBase : DomainEntity, IEntityBase<Guid>
{
    [Key]
    public Guid Id { get; set; } = Guid.Empty;

}
