using System.Text.RegularExpressions;

namespace TravelConnect.Domain.ValueObjects;

public partial class Email
{
    private static readonly Regex EmailRegex = MyRegex();

    public string Value { get; private set; } = string.Empty;

    public void Initialize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty.", nameof(value));
        if (!EmailRegex.IsMatch(value))
            throw new ArgumentException("Invalid email format.", nameof(value));

        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex MyRegex();
}
