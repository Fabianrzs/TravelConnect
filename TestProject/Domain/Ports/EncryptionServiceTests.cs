using Moq;
using TravelConnect.Domain.Ports;

namespace TestProject.Domain.Ports;

[TestFixture]
public class EncryptionServiceTests
{
    private Mock<IEncryptionService> _mockEncryptionService;

    [SetUp]
    public void Setup()
    {
        _mockEncryptionService = new Mock<IEncryptionService>();
    }

    [Test]
    public void Encrypt_WithValidPlainText_ReturnsEncryptedText()
    {
        var plainText = "test plain text";
        var encryptedText = "encrypted text";

        _mockEncryptionService
            .Setup(service => service.Encrypt(plainText))
            .Returns(encryptedText);

        var result = _mockEncryptionService.Object.Encrypt(plainText);

        Assert.That(result, Is.EqualTo(encryptedText));
        _mockEncryptionService.Verify(service => service.Encrypt(plainText), Times.Once);
    }

    [Test]
    public void Decrypt_WithValidCipherText_ReturnsPlainText()
    {
        var cipherText = "encrypted text";
        var plainText = "test plain text";

        _mockEncryptionService
            .Setup(service => service.Decrypt(cipherText))
            .Returns(plainText);

        var result = _mockEncryptionService.Object.Decrypt(cipherText);

        Assert.That(result, Is.EqualTo(plainText));
        _mockEncryptionService.Verify(service => service.Decrypt(cipherText), Times.Once);
    }

    [Test]
    public void Encrypt_WithNullPlainText_ThrowsException()
    {
        _mockEncryptionService
            .Setup(service => service.Encrypt(null!))
            .Throws(new System.ArgumentNullException("plainText"));

        Assert.Throws<System.ArgumentNullException>(() => _mockEncryptionService.Object.Encrypt(null!));
    }

    [Test]
    public void Decrypt_WithNullCipherText_ThrowsException()
    {
        _mockEncryptionService
            .Setup(service => service.Decrypt(null!))
            .Throws(new System.ArgumentNullException("cipherText"));

        Assert.Throws<System.ArgumentNullException>(() => _mockEncryptionService.Object.Decrypt(null!));
    }
}