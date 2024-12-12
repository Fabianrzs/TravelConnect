namespace TravelConnect.Domain.Ports.Security;

public interface ITokenService
{
    string GenerateToken(string username, Guid userId, IEnumerable<string> roles);
    bool ValidateToken(string token);
}
