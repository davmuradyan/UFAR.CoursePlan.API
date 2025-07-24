using UFAR.CoursePlan.API.Data.Entities.Infrastructure;
using UFAR.CoursePlan.API.Data.Entities.Presons;
using UFAR.CoursePlan.API.Data.Entities.Uni;

namespace UFAR.CoursePlan.API.Data.Entities.ManyToManyTables {
    public class SubjectProfessorEntity : MainAbstractEntity {
        public int SubjectId { get; set; }
        public SubjectEntity? Subject { get; set; }
        public int? ProfessorId { get; set; }
        public ProfessorEntity? Professor { get; set; }
        public int? ChairId { get; set; }
        public ChairEntity? Chair { get; set; }
        public byte GroupNumber { get; set; }
        public GroupType GroupType { get; set; }
    }

    public enum GroupType {
        CM = 0,
        CTD = 1,
        TD = 2,
        TP = 3,
        CTP = 4,
        TPS = 5,
        Project = 6
    }
}