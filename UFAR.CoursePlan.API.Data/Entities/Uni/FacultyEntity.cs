using UFAR.CoursePlan.API.Data.Entities.Presons;

namespace UFAR.CoursePlan.API.Data.Entities.Uni {
    public class FacultyEntity : MainAbstractEntity {
        public required string Name { get; set; }
        public int DeanId { get; set; }
        public DeanEntity? Dean { get; set; }
    }
}
