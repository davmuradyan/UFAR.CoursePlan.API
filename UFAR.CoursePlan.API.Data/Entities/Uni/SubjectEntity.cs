namespace UFAR.CoursePlan.API.Data.Entities.Uni {
    public class SubjectEntity : MainAbstractEntity {
        public required string Name { get; set; }
        public required string UE { get; set; }
        public double CM { get; set; }
        public double CTD { get; set; }
        public double TD { get; set; }
        public double TP { get; set; }
        public double CTP { get; set; }
        public double TPS { get; set; }
        public double Project { get; set; }
        public int BlockId { get; set; }
        public BlockEntity? Block { get; set; }
    }
}
