using UFAR.CoursePlan.API.Data.Entities.Presons;

namespace UFAR.CoursePlan.API.Data.Entities.Accounts {
    public class ProfessorAccountEntity : AbstractAccount {
        public int ProfessorId { get; set; }
        public ProfessorEntity? Professor { get; set; }
    }
}