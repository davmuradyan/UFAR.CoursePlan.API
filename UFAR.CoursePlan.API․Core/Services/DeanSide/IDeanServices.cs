using UFAR.CoursePlan.API.Data;
using UFAR.CoursePlan.API.Data.Entities.Presons;
using UFAR.CoursePlan.API_Core.CreatingDtos;
using UFAR.CoursePlan.API_Core.DTOs;
using UFAR.CoursePlan.API.Data.SendReceiveDtos.ReceiveDtos;

namespace UFAR.CoursePlan.API_Core.Services.DeanSide {
    public interface IDeanServices {
        Task<bool> CreateDean(DeanDto dean);
        Task<bool> CreateProfessors(int deanId, List<CreateProfDto> professors);
        Task<int> TryLoginFromDean(DeanLoginDto loginData);
        Task<List<ProfessorEntity>> GetProfessors(int deanId);
        Task<bool> ApplyDataTrackerChanges(int deanId, DataTrackerDto dataTrackerDto);
    }
}
