namespace TravelConnect.Domain.Extensions;
public static class DateTimeExtensions
{
    /// <summary>
    /// Verifica si una fecha está dentro de un rango.
    /// </summary>
    public static bool IsInRange(this DateTime date, DateTime startDate, DateTime endDate)
    {
        return date >= startDate && date <= endDate;
    }
}
