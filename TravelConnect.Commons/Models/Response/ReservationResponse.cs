using TravelConnect.Commons.Models.Request;

namespace TravelConnect.Commons.Models.Response;

public class ReservationResponse
{
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public RoomResponse Room { get; set; } = new();
    public ICollection<GuestRequest> Guests { get; set; } = [];
    public EmergencyContactRequest EmergencyContact { get; set; } = new();

}
