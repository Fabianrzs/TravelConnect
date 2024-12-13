using TravelConnect.Domain.Entities.Base;

namespace TravelConnect.Domain.ValueObjects;

public class Address: EntityBase
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string StateAdrees { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}
