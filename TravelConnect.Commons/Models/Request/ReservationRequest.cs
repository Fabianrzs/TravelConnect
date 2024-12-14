namespace TravelConnect.Commons.Models.Request;

public class ReservationRequest
{
    public Guid? Id { get; set; }
    public Guid RoomId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public List<GuestRequest> Guests { get; set; } = [];
    public EmergencyContactRequest EmergencyContact { get; set; } = new();
}
