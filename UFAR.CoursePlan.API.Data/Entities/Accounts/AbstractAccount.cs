namespace UFAR.CoursePlan.API.Data.Entities.Accounts {
    public abstract class AbstractAccount : MainAbstractEntity {
        public required string Password { get; set; }
    }
}
