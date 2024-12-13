using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TravelConnect.Application.Services;
using TravelConnect.Commons.Models.Request;
using TravelConnect.Commons.Models.Response;

namespace TravelConnect.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class HotelController(HotelService hotelService) : ControllerBase
{
    /// <summary>
    /// Crea un nuevo hotel.
    /// </summary>
    /// <param name="hotelRequest">Datos del hotel a crear.</param>
    /// <returns>Confirmación de la creación.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateHotel([FromBody] HotelRequest hotelRequest)
    {
        await hotelService.CreateHotelAsync(hotelRequest);
        return CreatedAtAction(nameof(GetHotelById), new { id = hotelRequest.Id }, hotelRequest);
    }

    /// <summary>
    /// Actualiza los datos de un hotel.
    /// </summary>
    /// <param name="hotel">Datos actualizados del hotel.</param>
    /// <returns>Confirmación de la actualización.</returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateHotel([FromBody] HotelRequest hotelm, Guid hotelId)
    {
        await hotelService.UpdateHotelAsync(hotelId, hotelm);
        return NoContent();
    }

    /// <summary>
    /// Deshabilita un hotel.
    /// </summary>
    /// <param name="id">Identificador único del hotel.</param>
    /// <returns>Confirmación de la deshabilitación.</returns>
    [HttpPatch("{id:guid}/disable")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DisableHotel(Guid id)
    {
        await hotelService.DisableHotelAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Habilita un hotel.
    /// </summary>
    /// <param name="id">Identificador único del hotel.</param>
    /// <returns>Confirmación de la habilitación.</returns>
    [HttpPatch("{id:guid}/enable")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EnableHotel(Guid id)
    {
        await hotelService.EnaleHotelAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Obtiene todos los hoteles.
    /// </summary>
    /// <returns>Lista de hoteles.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<HotelResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllHotels()
    {
        var response = await hotelService.GetAllHotelsAsync();
        return Ok(response);
    }

    /// <summary>
    /// Obtiene un hotel por su ID.
    /// </summary>
    /// <param name="id">Identificador único del hotel.</param>
    /// <returns>Información del hotel.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(HotelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotelById(Guid id)
    {
        var response = await hotelService.GetHotelByIdAsync(id);
        return Ok(response);
    }
}
