namespace UFAR.CoursePlan.API.Data.Entities.Uni {
    public class BlockEntity : MainAbstractEntity {
        public byte Number { get; set; }
        public Year Year { get; set; }
        public Semester Semester { get; set; }
        public int FacultyId { get; set; }
        public FacultyEntity? Faculty { get; set; }
    }

    public enum Year {
        LA = 0,
        L1 = 1,
        L2 = 2,
        L3 = 3,
        M1 = 4,
        M2 = 5,
    }

    public enum Semester {
        Semester1 = 0,
        Semester2 = 1,
    }
}
