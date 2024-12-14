namespace TravelConnect.Commons.Models.Response;

public class RoomResponse
{
    public Guid Id { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty; 
    public decimal BaseCost { get; set; }
    public decimal Taxes { get; set; }
    public bool IsEnabled { get; set; }
    public string Location { get; set; } = string.Empty;
    public bool HasActiveReservations { get; set; } 
}
