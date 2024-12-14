using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TravelConnect.Commons.Models.Request;
using TravelConnect.Commons.Models.Response;
using TravelConnect.Domain.Services;

namespace TravelConnect.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class TravelAgentController(TravelAgentService travelAgentService) : ControllerBase
{

    /// <summary>
    /// Registro de un nuevo agente de viajes
    /// </summary>
    /// <param name="request">Datos del agente de viaje</param>
    /// <returns>Información del agente registrado</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(TravelAgentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] TravelAgentRequest request)
    {
        var response = await travelAgentService.RegisterAsync(request);
        return CreatedAtAction(nameof(Register), new { id = response.Id }, response);
    }

    /// <summary>
    /// Inicio de sesión de un agente de viajes
    /// </summary>
    /// <param name="request">Credenciales del agente de viaje</param>
    /// <returns>Token JWT e información básica del agente</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] TravelAgentRequest request)
    {
        var response = await travelAgentService.LoginAsync(request);
        return Ok(response);
    }

    /// <summary>
    /// Obtiene la lista de todos los agentes de viaje
    /// </summary>
    /// <returns>Lista de agentes de viaje</returns>
    [HttpGet]
    [Authorize] // Requiere autenticación
    [ProducesResponseType(typeof(IEnumerable<TravelAgentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAll()
    {
        var response = await travelAgentService.GetAllAsync();
        return Ok(response);
    }
}
