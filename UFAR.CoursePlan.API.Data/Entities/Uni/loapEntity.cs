using UFAR.CoursePlan.API.Data.Entities.Infrastructure;

namespace UFAR.CoursePlan.API.Data.Entities.Uni {
    public class LoapEntity : AbstractUni {
        public LoapType LoapType { get; set; }
        public required string Value { get; set; }
    }

    public enum LoapType {
        Knowledge = 0,
        Skill = 1,
        SoftSkill = 2
    }
}
