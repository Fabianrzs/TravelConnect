using System.ComponentModel.DataAnnotations;
using TravelConnect.Domain.Entities;
using TravelConnect.Domain.Enums;

namespace TestProject.Domain.Entities;

[TestFixture]
public class RoomTests
{
    private Room _room;

    [SetUp]
    public void Setup()
    {
        _room = new Room();
    }

    [Test]
    public void Initialize_WithValidData_SetsProperties()
    {
        var hotelId = Guid.NewGuid();
        var roomType = "Suite";
        var baseCost = 100m;
        var taxes = 10m;
        var location = "Floor 1";

        _room.Initialize(hotelId, roomType, baseCost, taxes, location);

        Assert.Multiple(() =>
        {
            Assert.That(_room.HotelId, Is.EqualTo(hotelId));
            Assert.That(_room.RoomType, Is.EqualTo(RoomType.Suite));
            Assert.That(_room.BaseCost, Is.EqualTo(baseCost));
            Assert.That(_room.Taxes, Is.EqualTo(taxes));
            Assert.That(_room.Location, Is.EqualTo(location));
        });
    }

    [Test]
    public void Initialize_WithNegativeBaseCost_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _room.Initialize(Guid.NewGuid(), "Single", -1, 10m, "Location"));
    }

    [Test]
    public void Disable_WithActiveReservations_ThrowsValidationException()
    {
        _room.Reservations =
         [
             new ()
             {
                 Status = ReservationStatus.Confirmed,
                 StartDate = DateTime.UtcNow.AddDays(-1),
                 EndDate = DateTime.UtcNow.AddDays(1)
             }
         ];

        Assert.Throws<TravelConnect.Domain.Exceptions.ValidationException>(() => _room.Disable());
    }

    [Test]
    public void IsAvailable_WithOverlappingReservation_ReturnsFalse()
    {
        _room.Reservations =
         [
             new Reservation
             {
                 Status = ReservationStatus.Confirmed,
                 StartDate = DateTime.UtcNow.AddDays(-1),
                 EndDate = DateTime.UtcNow.AddDays(1)
             }
         ];

        var result = _room.IsAvailable(DateTime.UtcNow, DateTime.UtcNow.AddDays(2), 2);

        Assert.That(result, Is.False);
    }

    [Test]
    public void CalculateCost_WithValidDates_ReturnsCorrectCost()
    {
        _room.BaseCost = 100m;
        _room.Taxes = 50m;

        var cost = _room.CalculateCost(DateTime.UtcNow, DateTime.UtcNow.AddDays(3));

        Assert.That(cost, Is.EqualTo(350m));
    }
}
