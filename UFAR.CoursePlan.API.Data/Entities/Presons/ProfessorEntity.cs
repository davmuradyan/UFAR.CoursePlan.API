using UFAR.CoursePlan.API.Data.Entities.Infrastructure;

namespace UFAR.CoursePlan.API.Data.Entities.Presons {
    public class ProfessorEntity : AbstractPerson {
        public required string Phone { get; set; }
        public int? FacultyId { get; set; }
        public FacultyEntity? Faculty { get; set; }
        public int? ChairId { get; set; }
        public ChairEntity? Chair { get; set; }
    }
}
