namespace TravelConnect.Commons.Models.Response;

public class HotelResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public List<RoomResponse> Rooms { get; set; } = new();
}
