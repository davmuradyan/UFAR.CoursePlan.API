using UFAR.CoursePlan.API.Data.Entities.Infrastructure;

namespace UFAR.CoursePlan.API.Data.Entities.Uni {
    public abstract class AbstractUni : MainAbstractEntity {
        public int FacultyId { get; set; }
        public FacultyEntity? Faculty { get; set; }
    }
}
