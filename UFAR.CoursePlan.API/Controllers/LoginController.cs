using Microsoft.AspNetCore.Mvc;
using UFAR.CoursePlan.API_Core.DTOs;
using UFAR.CoursePlan.API_Core.Services.ChairpersonSide;
using UFAR.CoursePlan.API_Core.Services.DeanSide;


namespace UFAR.CoursePlan.API.Controllers {
    public class LoginController : Controller {
        IDeanServices deanServices;
        IChairpersonSide chairpersonSide;
        public LoginController(IDeanServices deanServices, IChairpersonSide chairpersonSide)
        {
            this.deanServices = deanServices;
            this.chairpersonSide = chairpersonSide;
        }

        [HttpPost("CreateDean")]
        public async Task<IActionResult> CreateDean([FromBody] DeanDto dean) {
            if (await deanServices.CreateDean(dean)) {
                return Ok("Dean created successfully");
            } else {
                return BadRequest("Failed to create dean");
            }
        }

        [HttpPost("TryLoginFromDean")]
        public async Task<IActionResult> TryLoginFromDean([FromBody] DeanLoginDto loginData)
        {
            var result = await deanServices.TryLoginFromDean(loginData);
            if (result == LoginResult.Success)
            {
                return Ok("Login successful");
            }
            else if (result == LoginResult.InvalidCredentials)
            {
                return Unauthorized("Invalid credentials");
            }
            else
            {
                return StatusCode(500, "An error occurred during login");
            }
        }
        [HttpPost("CreateChairperson")]
        public async Task<IActionResult> CreateChairperson([FromBody] ChairpersonDto chairperson) {
            if (await chairpersonSide.CreateChairperson(chairperson)) {
                return Ok("Chairperson created successfully");
            } else {
                return BadRequest("Failed to create chairperson");
            }
        }
    }
}