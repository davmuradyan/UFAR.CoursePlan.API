using UFAR.CoursePlan.API_Core.DTOs;

namespace UFAR.CoursePlan.API_Core.Services.AdminServices {
    public interface IAdminServices {
        // Creating a university
        Task<bool> CreateUniversity(UniversityDto university);
        // Creating a faculty
        Task<bool> CreateFaculty(FacultyDto faculty);
        // Creating a chair
        Task<bool> CreateChair(ChairDto chair);
    }
}
