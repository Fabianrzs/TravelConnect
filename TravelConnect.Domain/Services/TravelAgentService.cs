using System.ComponentModel.DataAnnotations;
using TravelConnect.Commons.Models.Request;
using TravelConnect.Commons.Models.Response;
using TravelConnect.Domain.Entities;
using TravelConnect.Domain.Ports;
using TravelConnect.Domain.Ports.Persistence;
using TravelConnect.Domain.Ports.Security;

namespace TravelConnect.Domain.Services;
[DomainService]
public class TravelAgentService( IRepository<TravelAgent> repository, IUnitOfWork unitOfWork,
    ITokenService tokenService, IPasswordHasher passwordHasher,  IEncryptionService encryptionService)
{

    public async Task<TravelAgentResponse> RegisterAsync(TravelAgentRequest request)
    {
        var encryptedAgencyName = encryptionService.Encrypt(request.Username);

        var find = await repository.FindAsync(a => a.Username == encryptedAgencyName);
        
        if (find.Any())
            throw new ValidationException("Username is already taken.");

        var hashedPassword = passwordHasher.HashPassword(request.Password);

        var agent = new TravelAgent
        {
            Username = request.Username,
            PasswordHash = hashedPassword,
            IsActive = true,
        };

        await repository.AddAsync(agent);
        await unitOfWork.CommitAsync();

        return new TravelAgentResponse
        {
            Id = agent.Id,
            Username = agent.Username
        };
    }

    public async Task<LoginResponse> LoginAsync(TravelAgentRequest request)
    {
        var encryptedAgencyName = encryptionService.Encrypt(request.Username);

        var agent = (await repository.FindAsync(a => a.Username == encryptedAgencyName)).FirstOrDefault();

        if (agent == null || !passwordHasher.VerifyPassword(agent.PasswordHash, request.Password))
            throw new UnauthorizedAccessException("Invalid username or password.");

        var token = tokenService.GenerateToken(agent.Username, agent.Id);

        return new()
        {
            Token = token,
            TravelAgent = new ()
            {
                Id = agent.Id,
                Username = encryptionService.Decrypt(agent.Username),
            }
        };
    }
    public async Task<IEnumerable<TravelAgentResponse>> GetAllAsync()
    {
        var agents = await repository.GetAllAsync();

        return agents.Select(agent => new TravelAgentResponse
        {
            Id = agent.Id,
            Username =   encryptionService.Decrypt(agent.Username),
        });
    }
}
