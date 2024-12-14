using TravelConnect.Domain.Entities;

namespace TestProject.Domain.Entities;


[TestFixture]
public class ReservationTests
{
    private Reservation _reservation;

    [SetUp]
    public void Setup()
    {
        _reservation = new Reservation();
    }

    [Test]
    public void ValidateDates_WithInvalidDates_ThrowsException()
    {
        _reservation.StartDate = DateTime.UtcNow.AddDays(1);
        _reservation.EndDate = DateTime.UtcNow;

        Assert.Throws<Exception>(() => _reservation.ValidateDates());
    }

    [Test]
    public void ValidateDates_WithValidDates_DoesNotThrow()
    {
        _reservation.StartDate = DateTime.UtcNow;
        _reservation.EndDate = DateTime.UtcNow.AddDays(1);

        Assert.DoesNotThrow(() => _reservation.ValidateDates());
    }
   
}
