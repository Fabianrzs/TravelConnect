using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TravelConnect.Application.Services;
using TravelConnect.Domain.Entities;

namespace TravelConnect.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ReservationController(ReservationService reservationService) : ControllerBase
{
    /// <summary>
    /// Obtiene todas las reservas de una habitación específica.
    /// </summary>
    /// <param name="roomId">Identificador único de la habitación.</param>
    /// <returns>Lista de reservas asociadas a la habitación.</returns>
    [HttpGet("room/{roomId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<Reservation>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReservationsByRoom(Guid roomId)
    {
        var reservations = await reservationService.GetReservationsByRoomAsync(roomId);
        return reservations.Any() ? Ok(reservations) : NotFound("No reservations found for the specified room.");
    }

    /// <summary>
    /// Obtiene el detalle de una reserva específica.
    /// </summary>
    /// <param name="reservationId">Identificador único de la reserva.</param>
    /// <returns>Información detallada de la reserva.</returns>
    [HttpGet("{reservationId:guid}")]
    [ProducesResponseType(typeof(Reservation), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReservationDetails(Guid reservationId)
    {
        var reservation = await reservationService.GetReservationDetailsAsync(reservationId);
        return Ok(reservation);
    }

    /// <summary>
    /// Obtiene todas las reservas asociadas a un hotel específico.
    /// </summary>
    /// <param name="hotelId">Identificador único del hotel.</param>
    /// <returns>Lista de reservas asociadas al hotel.</returns>
    [HttpGet("hotel/{hotelId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<Reservation>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReservationsByHotel(Guid hotelId)
    {
        var reservations = await reservationService.GetReservationsByHotelAsync(hotelId);
        return reservations.Any() ? Ok(reservations) : NotFound("No reservations found for the specified hotel.");
    }
}
