namespace UFAR.CoursePlan.API.Data.Entities.Uni {
    public class loapEntity : MainAbstractEntity {
        public LoapType LoapType { get; set; }
        public required string Value { get; set; }
        public int FacultyId { get; set; }
        public FacultyEntity? Faculty { get; set; }
    }

    public enum LoapType {
        Knowledge = 0,
        Skill = 1,
        SoftSkill = 2
    }
}
