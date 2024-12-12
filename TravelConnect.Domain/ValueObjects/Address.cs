namespace TravelConnect.Domain.ValueObjects;

public class Address
{
    public string Street { get; }
    public string City { get; private set; } = null!;
    public string State { get; }
    public string ZipCode { get; }

    public Address(string street, string city, string state, string zipCode)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street cannot be empty.", nameof(street));
        if (string.IsNullOrWhiteSpace(state))
            throw new ArgumentException("State cannot be empty.", nameof(state));
        if (string.IsNullOrWhiteSpace(zipCode))
            throw new ArgumentException("ZipCode cannot be empty.", nameof(zipCode));

        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    public override string ToString()
    {
        return $"{Street}, {City}, {State}, {ZipCode}";
    }
}
