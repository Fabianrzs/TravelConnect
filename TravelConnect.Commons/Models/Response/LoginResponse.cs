namespace TravelConnect.Commons.Models.Response;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public TravelAgentResponse TravelAgent { get; set; } = null!;
}
