using UFAR.CoursePlan.API.Data.Entities.Presons;

namespace UFAR.CoursePlan.API.Data.Entities.Accounts {
    public class ProfessorAccountEntity : MainAbstractEntity {
        public int ProfessorId { get; set; }
        public ProfessorEntity? Professor { get; set; }
        public required string Password { get; set; }
    }
}
