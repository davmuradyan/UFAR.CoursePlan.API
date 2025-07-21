namespace UFAR.CoursePlan.API.Data.Entities {
    public abstract class MainAbstractEntity {
        public int Id { get; init; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
    }
}
