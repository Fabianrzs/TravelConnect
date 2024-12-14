namespace TravelConnect.Domain.Ports.Security;

public interface ITokenService
{
    string GenerateToken(string username, Guid userId);
    bool ValidateToken(string token);
}
