using Microsoft.AspNetCore.Mvc;
using UFAR.CoursePlan.API_Core.Services.DeanSide;
using UFAR.CoursePlan.API.Data.SendReceiveDtos.ReceiveDtos;
using Microsoft.Extensions.Logging;

namespace UFAR.CoursePlan.API.Controllers;
public class DeanOperationsController : Controller
{
    private readonly IDeanServices _deanServices;
    private readonly ILogger<DeanOperationsController> _logger;

    public DeanOperationsController(
        IDeanServices deanServices,
        ILogger<DeanOperationsController> logger)
    {
        _deanServices = deanServices;
        _logger = logger;
    }
    
    [HttpPost("SaveDeanChanges")]
    public async Task<IActionResult> SaveDeanChanges([FromBody] DataTrackerDto dataTrackerDto, [FromQuery] int deanId)
    {
        _logger.LogInformation("Received SaveDeanChanges request");
        _logger.LogInformation($"{dataTrackerDto.CreateProfessorList.Count}");

        if (dataTrackerDto == null)
        {
            return BadRequest("DataTracker is required");
        }

        if (deanId <= 0)
        {
            return BadRequest("Valid deanId is required");
        }

        var result = await _deanServices.ApplyDataTrackerChanges(deanId, dataTrackerDto);

        if (result)
        {
            return Ok(new { success = true, message = "Changes applied successfully" });
        }
        
        return StatusCode(500, new { success = false, message = "Failed to apply changes" });
    }
}