namespace TravelConnect.Domain.Exceptions;

public class UnauthorizedAccessException(string message) : DomainException(message)
{
}
