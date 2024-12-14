namespace TravelConnect.Commons.Models.Request;
public class HotelRequest
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string StateAddress { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}
