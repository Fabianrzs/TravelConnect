namespace TravelConnect.Commons.Models.Response;

public class RoomResponse
{
    public Guid Id { get; set; }
    public string RoomType { get; set; } = string.Empty; // Ejemplo: Single, Double, Suite
    public decimal BaseCost { get; set; }
    public decimal Taxes { get; set; }
    public bool IsEnabled { get; set; }
    public string Location { get; set; } = string.Empty; // Ubicación de la habitación (Ej: Piso, Edificio)
    public bool HasActiveReservations { get; set; } 
}
