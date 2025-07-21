using UFAR.CoursePlan.API.Data.Entities.Uni;

namespace UFAR.CoursePlan.API.Data.Entities.Presons {
    public class ProfessorEntity : MainAbstractEntity {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Phone { get; set; }
        public int FacultyId { get; set; }
        public FacultyEntity? Faculty { get; set; }
    }
}
