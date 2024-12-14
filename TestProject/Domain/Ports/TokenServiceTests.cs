using Moq;
using TravelConnect.Domain.Ports.Security;

namespace TestProject.Domain.Ports;

[TestFixture]
public class TokenServiceTests
{
    private Mock<ITokenService> _mockTokenService;

    [SetUp]
    public void Setup()
    {
        _mockTokenService = new Mock<ITokenService>();
    }

    [Test]
    public void GenerateToken_WithValidInputs_ReturnsToken()
    {
        var username = "testuser";
        var userId = Guid.NewGuid();
        var token = "generatedToken123";

        _mockTokenService
            .Setup(service => service.GenerateToken(username, userId))
            .Returns(token);

        var result = _mockTokenService.Object.GenerateToken(username, userId);

        Assert.That(result, Is.EqualTo(token));
        _mockTokenService.Verify(service => service.GenerateToken(username, userId), Times.Once);
    }

    [Test]
    public void ValidateToken_WithValidToken_ReturnsTrue()
    {
        var token = "validToken123";

        _mockTokenService
            .Setup(service => service.ValidateToken(token))
            .Returns(true);

        var result = _mockTokenService.Object.ValidateToken(token);

        Assert.That(result, Is.True);
        _mockTokenService.Verify(service => service.ValidateToken(token), Times.Once);
    }

    [Test]
    public void ValidateToken_WithInvalidToken_ReturnsFalse()
    {
        var token = "invalidToken123";

        _mockTokenService
            .Setup(service => service.ValidateToken(token))
            .Returns(false);

        var result = _mockTokenService.Object.ValidateToken(token);

        Assert.That(result, Is.False);
        _mockTokenService.Verify(service => service.ValidateToken(token), Times.Once);
    }
}
