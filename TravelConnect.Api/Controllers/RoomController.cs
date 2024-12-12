using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TravelConnect.Application.Services;
using TravelConnect.Domain.Entities;

namespace TravelConnect.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class RoomController(RoomService roomService) : ControllerBase
{

    /// <summary>
    /// Obtiene todas las habitaciones asociadas a un hotel.
    /// </summary>
    /// <param name="hotelId">Identificador único del hotel.</param>
    /// <returns>Lista de habitaciones del hotel.</returns>
    [HttpGet("hotel/{hotelId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<Room>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoomsByHotel(Guid hotelId)
    {
        if (hotelId == Guid.Empty)
            return BadRequest(new { Message = "Hotel ID is invalid." });

        var rooms = await roomService.GetRoomsByHotelAsync(hotelId);
        return rooms.Any() ? Ok(rooms) : NotFound(new { Message = "No rooms found for the specified hotel." });
    }

    /// <summary>
    /// Obtiene el detalle de una habitación específica.
    /// </summary>
    /// <param name="id">Identificador único de la habitación.</param>
    /// <returns>Información detallada de la habitación.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Room), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoomById(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { Message = "Room ID is invalid." });

        var room = await roomService.GetRoomByIdAsync(id);
        return room != null ? Ok(room) : NotFound(new { Message = "Room not found." });
    }

    /// <summary>
    /// Crea una nueva habitación en un hotel.
    /// </summary>
    /// <param name="room">Datos de la habitación a crear.</param>
    /// <returns>Confirmación de la creación.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRoom([FromBody] Room room)
    {
        if (room == null)
            return BadRequest(new { Message = "Room data is required." });

        await roomService.CreateRoomAsync(room);
        return CreatedAtAction(nameof(GetRoomById), new { id = room.Id }, room);
    }

    /// <summary>
    /// Actualiza los datos de una habitación específica.
    /// </summary>
    /// <param name="room">Datos actualizados de la habitación.</param>
    /// <returns>Confirmación de la actualización.</returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRoom([FromBody] Room room)
    {
        if (room == null || room.Id == Guid.Empty)
            return BadRequest(new { Message = "Valid room data is required." });

        await roomService.UpdateRoomAsync(room);
        return NoContent();
    }

    /// <summary>
    /// Deshabilita una habitación.
    /// </summary>
    /// <param name="id">Identificador único de la habitación.</param>
    /// <returns>Confirmación de la deshabilitación.</returns>
    [HttpPatch("{id:guid}/disable")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DisableRoom(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { Message = "Room ID is invalid." });

        await roomService.DisableRoomAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Habilita una habitación.
    /// </summary>
    /// <param name="id">Identificador único de la habitación.</param>
    /// <returns>Confirmación de la habilitación.</returns>
    [HttpPatch("{id:guid}/enable")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EnableRoom(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { Message = "Room ID is invalid." });

        await roomService.EnableRoomAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Verifica si una habitación tiene reservas activas.
    /// </summary>
    /// <param name="id">Identificador único de la habitación.</param>
    /// <returns>True si la habitación tiene reservas activas, de lo contrario, false.</returns>
    [HttpGet("{id:guid}/has-active-reservations")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> HasActiveReservations(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { Message = "Room ID is invalid." });

        var hasActiveReservations = await roomService.HasActiveReservationsAsync(id);
        return Ok(new { RoomId = id, HasActiveReservations = hasActiveReservations });
    }
}
