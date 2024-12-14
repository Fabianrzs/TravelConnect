using Moq;
using TravelConnect.Domain.Ports.Security;

namespace TestProject.Domain.Ports;

[TestFixture]
public class PasswordHasherTests
{
    private Mock<IPasswordHasher> _mockPasswordHasher;

    [SetUp]
    public void Setup()
    {
        _mockPasswordHasher = new Mock<IPasswordHasher>();
    }

    [Test]
    public void HashPassword_WithValidPassword_ReturnsHashedPassword()
    {
        var password = "MySecurePassword";
        var hashedPassword = "hashedPassword123";

        _mockPasswordHasher
            .Setup(hasher => hasher.HashPassword(password))
            .Returns(hashedPassword);

        var result = _mockPasswordHasher.Object.HashPassword(password);

        Assert.That(result, Is.EqualTo(hashedPassword));
        _mockPasswordHasher.Verify(hasher => hasher.HashPassword(password), Times.Once);
    }

    [Test]
    public void VerifyPassword_WithMatchingPassword_ReturnsTrue()
    {
        var hashedPassword = "hashedPassword123";
        var providedPassword = "MySecurePassword";

        _mockPasswordHasher
            .Setup(hasher => hasher.VerifyPassword(hashedPassword, providedPassword))
            .Returns(true);

        var result = _mockPasswordHasher.Object.VerifyPassword(hashedPassword, providedPassword);

        Assert.That(result, Is.True);
        _mockPasswordHasher.Verify(hasher => hasher.VerifyPassword(hashedPassword, providedPassword), Times.Once);
    }

    [Test]
    public void VerifyPassword_WithNonMatchingPassword_ReturnsFalse()
    {
        var hashedPassword = "hashedPassword123";
        var providedPassword = "WrongPassword";

        _mockPasswordHasher
            .Setup(hasher => hasher.VerifyPassword(hashedPassword, providedPassword))
            .Returns(false);

        var result = _mockPasswordHasher.Object.VerifyPassword(hashedPassword, providedPassword);

        Assert.That(result, Is.False);
        _mockPasswordHasher.Verify(hasher => hasher.VerifyPassword(hashedPassword, providedPassword), Times.Once);
    }
}
