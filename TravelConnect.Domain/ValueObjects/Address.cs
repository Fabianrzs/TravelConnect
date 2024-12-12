using TravelConnect.Domain.Entities.Base;

namespace TravelConnect.Domain.ValueObjects;

public class Address: EntityBase
{
    public string Street { get; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string StateAdrees { get; } = string.Empty;
    public string ZipCode { get; } = string.Empty;
}
