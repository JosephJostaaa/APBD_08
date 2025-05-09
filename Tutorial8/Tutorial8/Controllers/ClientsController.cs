using Microsoft.AspNetCore.Mvc;
using Tutorial8.Models.DTOs;
using Tutorial8.Services;

namespace Tutorial8.Controllers;

[Route("api/[controller]")]
public class ClientsController : Controller
{
    private readonly IClientsService _clientsService;

    public ClientsController(IClientsService clientsService)
    {
        _clientsService = clientsService;
    }

    [HttpGet("{id}/trips")]
    public async Task<IActionResult> GetTripsByClientId(int id, CancellationToken ct)
    {
        var trips = await _clientsService.GetTripsByClientIdAsync(id, ct);
        return Ok(trips);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddClient([FromBody] ClientRequest clientRequest, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var clientId = await _clientsService.AddClientAsync(clientRequest, ct);
        
        if (clientId == -1)
        {
            return BadRequest("The body of the request is empty");
        }
        return Created($"/api/clients/{clientId}", clientId);
    }

    [HttpPut("{clientId}/trips/{tripId}")]
    public async Task<IActionResult> UpdateTrip(int clientId, int tripId, CancellationToken ct)
    {
        var result = await _clientsService.RegisterClientOnTripAsync(clientId, tripId, ct);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        
        return BadRequest(result);
        
    }
    
    [HttpDelete("{clientId}/trips/{tripId}")]
    public async Task<IActionResult> CancelRegistration(int clientId, int tripId, CancellationToken ct)
    {
        var result = await _clientsService.CancelRegistrationAsync(clientId, tripId, ct);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        
        return BadRequest(result);
        
    }
}