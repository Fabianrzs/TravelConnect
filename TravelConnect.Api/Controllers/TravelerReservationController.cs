using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TravelConnect.Application.Services;
using TravelConnect.Commons.Models.Request;
using TravelConnect.Domain.Entities;

namespace TravelConnect.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class TravelerReservationController(TravelerReservationService reservationService) : ControllerBase
{
    /// <summary>
    /// Busca hoteles disponibles según los criterios proporcionados.
    /// </summary>
    /// <param name="checkIn">Fecha de entrada al alojamiento.</param>
    /// <param name="checkOut">Fecha de salida del alojamiento.</param>
    /// <param name="numberOfGuests">Cantidad de personas que se alojarán.</param>
    /// <param name="city">Ciudad de destino.</param>
    /// <returns>Lista de hoteles disponibles.</returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<Hotel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchHotels(DateTime checkIn, DateTime checkOut, int numberOfGuests, string city)
    {
        if (string.IsNullOrWhiteSpace(city))
            return BadRequest(new { Message = "City is required." });

        var hotels = await reservationService.SearchHotelsAsync(checkIn, checkOut, numberOfGuests, city);
        return Ok(hotels);
    }

    /// <summary>
    /// Selecciona una habitación en un hotel.
    /// </summary>
    /// <param name="hotelId">Identificador del hotel.</param>
    /// <param name="roomId">Identificador de la habitación.</param>
    /// <returns>Detalles de la habitación seleccionada.</returns>
    [HttpGet("select-room")]
    [ProducesResponseType(typeof(Room), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SelectRoom(Guid hotelId, Guid roomId)
    {
        if (hotelId == Guid.Empty || roomId == Guid.Empty)
            return BadRequest(new { Message = "HotelId and RoomId are required." });

        var room = await reservationService.SelectRoomAsync(hotelId, roomId);
        return Ok(room);
    }

    /// <summary>
    /// Completa una reserva con los detalles proporcionados.
    /// </summary>
    /// <param name="reservation">Detalles de la reserva.</param>
    /// <returns>Confirmación de la reserva.</returns>
    [HttpPost("complete")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CompleteReservation([FromBody] ReservationRequest reservation)
    {
        if (reservation == null)
            return BadRequest(new { Message = "Reservation data is required." });

        await reservationService.CompleteReservationAsync(reservation);
        return CreatedAtAction(nameof(CompleteReservation), new { id = reservation.Id }, reservation);
    }

    /// <summary>
    /// Cancela una reserva.
    /// </summary>
    /// <param name="reservationId">Identificador de la reserva a cancelar.</param>
    /// <returns>Confirmación de la cancelación.</returns>
    [HttpPatch("{reservationId:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelReservation(Guid reservationId)
    {
        if (reservationId == Guid.Empty)
            return BadRequest(new { Message = "ReservationId is required." });

        await reservationService.CancelReservationAsync(reservationId);
        return NoContent();
    }
}
