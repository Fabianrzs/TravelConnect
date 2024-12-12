namespace TravelConnect.Domain.ValueObjects;

public class City
{
    public string Name { get; private set; } = string.Empty;

    public void Initialize(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("City name cannot be empty.", nameof(name));

        Name = name;
    }

}
