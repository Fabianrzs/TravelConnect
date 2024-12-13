using TravelConnect.Domain.Enums;

namespace TravelConnect.Domain.Extensions;
public static class RoomTypeExtensions
{
    /// <summary>
    /// Convierte el estado de la reserva en un string amigable.
    /// </summary>
    public static string ToFriendlyString(this RoomType type)
    {
        return type switch
        {
            RoomType.Family => "Family",
            RoomType.Deluxe => "Deluxe",
            RoomType.Double => "Double",
            RoomType.Single => "Single",
            RoomType.Suite => "Suite",
            _ => "Unknown"
        };
    }
}
