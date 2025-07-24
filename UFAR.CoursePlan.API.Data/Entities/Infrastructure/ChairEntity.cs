using UFAR.CoursePlan.API.Data.Entities.Presons;

namespace UFAR.CoursePlan.API.Data.Entities.Infrastructure {
    public class ChairEntity : AbstractInfrastructure {
        public int ChairpersonId { get; set; }
        public ChairpersonEntity? Chairperson { get; set; }
    }
}
