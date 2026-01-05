using UFAR.CoursePlan.API.Data;
using UFAR.CoursePlan.API.Data.Entities.Presons;
using UFAR.CoursePlan.API_Core.CreatingDtos;
using UFAR.CoursePlan.API_Core.DTOs;

namespace UFAR.CoursePlan.API_Core.Services.DeanSide {
    public interface IDeanServices {
        Task<bool> CreateDean(DeanDto dean);
        Task<bool> CreateProfessors(int deanId, List<CreateProfDto> professors);
        Task<int> TryLoginFromDean(DeanLoginDto loginData);
        Task<List<ProfessorEntity>> GetProfessors(int deanId);
    }

    public class DeanLoginDto {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public enum LoginResult {
        Success,
        InvalidCredentials,
        Error
    }
}
