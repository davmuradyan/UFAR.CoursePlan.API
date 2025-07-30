using Microsoft.AspNetCore.Mvc;
using UFAR.CoursePlan.API_Core.CreatingDtos;
using UFAR.CoursePlan.API_Core.DTOs;
using UFAR.CoursePlan.API_Core.Services.AdminServices;
using UFAR.CoursePlan.API_Core.Services.DeanSide;

namespace UFAR.CoursePlan.API.Controllers {
    public class CreationLogic : Controller {
        IAdminServices adminServices;
        IDeanServices deanServices;
        public CreationLogic(IAdminServices adminServices, IDeanServices deanServices) {
            this.adminServices = adminServices;
            this.deanServices = deanServices;
        }

        [HttpPost("CreateUniversity")]
        public async Task<IActionResult> CreateUniversity(UniversityDto uni) {
            bool result = await adminServices.CreateUniversity(uni);
            if (result) {
                return Ok("University created successfully.");
            } else {
                return BadRequest("Failed to create university.");
            }
        }

        [HttpPost("CreateFaculty")]
        public async Task<IActionResult> CreateFaculty(FacultyDto faculty) {
            bool result = await adminServices.CreateFaculty(faculty);
            if (result) {
                return Ok("Faculty created successfully.");
            } else {
                return BadRequest("Failed to create faculty.");
            }
        }

        [HttpPost("CreateChair")]
        public async Task<IActionResult> CreateChair(ChairDto chair) {
            bool result = await adminServices.CreateChair(chair);
            if (result) {
                return Ok("Chair created successfully.");
            } else {
                return BadRequest("Failed to create chair.");
            }
        }
        [HttpPost("CreateProfs")]
        public async Task<IActionResult> CreateProfessors([FromBody] CreateProfsDtoReceived dto) {
            bool result = await deanServices.CreateProfessors(dto.deanId, dto.profs);
            if (result) {
                return Ok("Professors saved successfully.");
            } else {
                return BadRequest("Failed to create professors. Or it was done partially.");
            }
        }
    }

    public record CreateProfsDtoReceived(int deanId, List<CreateProfDto> profs);
}
