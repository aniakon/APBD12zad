using APBDcw12.DTOs;
using APBDcw12.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBDcw12.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly IDbService _dbService;

    public TripsController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet]
    public async Task<ActionResult<tripPageDTO>> GetTrips(int page = 1, int pageSize = 10)
    {
        var res = await _dbService.getTripPagesAsync(page, pageSize);
        return Ok(res);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrip(int id)
    {
        try
        {
            await _dbService.deleteClientAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AddClientToTrip(int idTrip, [FromBody] clientTripDTO clientTripDto)
    {
        try
        {
            await _dbService.addClientToTripAsync(idTrip, clientTripDto);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}