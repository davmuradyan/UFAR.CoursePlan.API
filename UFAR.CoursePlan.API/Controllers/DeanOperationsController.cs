using Microsoft.AspNetCore.Mvc;
using UFAR.CoursePlan.API_Core.Services.DeanSide;
using UFAR.CoursePlan.API.Data.SendReceiveDtos.ReceiveDtos;

namespace UFAR.CoursePlan.API.Controllers;

public class DeanOperationsController : Controller
{
    IDeanServices _deanServices;
    public DeanOperationsController(IDeanServices deanServices)
    {
        _deanServices = deanServices;
    }
    
    [HttpPost("SaveDeanChanges")]
    public async Task<IActionResult> SaveDeanChanges([FromBody] DataTrackerDto dataTrackerDto, [FromQuery] int deanId)
    {
        Console.WriteLine("Received SaveDeanChanges request");
        Console.WriteLine(dataTrackerDto.CreateProfessorList.Count);
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
        else
        {
            return StatusCode(500, new { success = false, message = "Failed to apply changes" });
        }
    }
}