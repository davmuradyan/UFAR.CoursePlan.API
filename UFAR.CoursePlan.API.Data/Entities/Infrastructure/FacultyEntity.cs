using UFAR.CoursePlan.API.Data.Entities.Presons;

namespace UFAR.CoursePlan.API.Data.Entities.Infrastructure {
    public class FacultyEntity : AbstractInfrastructure {
        public int DeanId { get; set; }
        public DeanEntity? Dean { get; set; }
    }
}
