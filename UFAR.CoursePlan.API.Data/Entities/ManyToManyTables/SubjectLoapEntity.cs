using UFAR.CoursePlan.API.Data.Entities.Uni;

namespace UFAR.CoursePlan.API.Data.Entities.ManyToManyTables {
    public class SubjectLoapEntity : MainAbstractEntity {
        public int SubjectId { get; set; }
        public SubjectEntity? Subject { get; set; }
        
        public int LoapId { get; set; }
        public LoapEntity? Loap { get; set; }
        public Level Level { get; set; }
    }

    public enum Level {
        L1 = 0,
        L2 = 1,
        L3 = 2
    }
}
