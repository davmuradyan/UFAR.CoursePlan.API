using UFAR.CoursePlan.API.Data;
using UFAR.CoursePlan.API_Core.DTOs;

namespace UFAR.CoursePlan.API_Core.Services.DeanSide {
    public interface IDeanServices {
        Task<bool> CreateDean(DeanDto dean);
        Task<bool> CreateProfessor(ProfessorDto professor, int deanId);
        Task<LoginResult> TryLoginFromDean(DeanLoginDto loginData);
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
