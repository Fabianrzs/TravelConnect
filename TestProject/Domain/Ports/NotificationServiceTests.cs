using Moq;
using TravelConnect.Domain.Ports.Notifications;

namespace TestProject.Domain.Ports.Notifications;

[TestFixture]
public class NotificationServiceTests
{
    private Mock<INotificationService> _mockNotificationService;

    [SetUp]
    public void Setup()
    {
        _mockNotificationService = new Mock<INotificationService>();
    }

    [Test]
    public async Task SendEmailAsync_WithValidParameters_CallsSendEmail()
    {
        var to = "test@example.com";
        var subject = "Test Subject";
        var body = "Test Body";

        _mockNotificationService
            .Setup(service => service.SendEmailAsync(to, subject, body))
            .Returns(Task.CompletedTask);

        await _mockNotificationService.Object.SendEmailAsync(to, subject, body);

        _mockNotificationService.Verify(
            service => service.SendEmailAsync(to, subject, body),
            Times.Once);
    }

    [Test]
    public async Task SendSmsAsync_WithValidParameters_CallsSendSms()
    {
        var phoneNumber = "+1234567890";
        var message = "Test SMS Message";

        _mockNotificationService
            .Setup(service => service.SendSmsAsync(phoneNumber, message))
            .Returns(Task.CompletedTask);

        await _mockNotificationService.Object.SendSmsAsync(phoneNumber, message);

        _mockNotificationService.Verify(
            service => service.SendSmsAsync(phoneNumber, message),
            Times.Once);
    }

    [Test]
    public void SendEmailAsync_WithInvalidParameters_ThrowsException()
    {
        string to = "";
        string subject = "Test Subject";
        string body = "Test Body";

        _mockNotificationService
            .Setup(service => service.SendEmailAsync(to, subject, body))
            .ThrowsAsync(new System.ArgumentException("Invalid email address"));

        Assert.ThrowsAsync<System.ArgumentException>(async () =>
            await _mockNotificationService.Object.SendEmailAsync(to, subject, body));
    }

    [Test]
    public void SendSmsAsync_WithInvalidPhoneNumber_ThrowsException()
    {
        string phoneNumber = "invalid_phone";
        string message = "Test SMS Message";

        _mockNotificationService
            .Setup(service => service.SendSmsAsync(phoneNumber, message))
            .ThrowsAsync(new System.ArgumentException("Invalid phone number"));

        Assert.ThrowsAsync<System.ArgumentException>(async () =>
            await _mockNotificationService.Object.SendSmsAsync(phoneNumber, message));
    }
}
