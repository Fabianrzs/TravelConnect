namespace TravelConnect.Commons.Models.Request;

public class RoomRequest
{
    public Guid HotelId { get; set; } 
    public string RoomType { get; set; } = string.Empty;
    public decimal BaseCost { get; set; } 
    public decimal Taxes { get; set; } 
    public string Location { get; set; } = string.Empty; 
    public bool IsEnabled { get; set; } = true; 
}
