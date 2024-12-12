using TravelConnect.Domain.Enums;

namespace TravelConnect.Domain.Extensions;
public static class ReservationStatusExtensions
{
    /// <summary>
    /// Convierte el estado de la reserva en un string amigable.
    /// </summary>
    public static string ToFriendlyString(this ReservationStatus status)
    {
        return status switch
        {
            ReservationStatus.Pending => "Pending",
            ReservationStatus.Confirmed => "Confirmed",
            ReservationStatus.Canceled => "Canceled",
            _ => "Unknown"
        };
    }

    /// <summary>
    /// Verifica si el estado de la reserva es final (Confirmed o Canceled).
    /// </summary>
    public static bool IsFinalStatus(this ReservationStatus status)
    {
        return status == ReservationStatus.Confirmed || status == ReservationStatus.Canceled;
    }
}
