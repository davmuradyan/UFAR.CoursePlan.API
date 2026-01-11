namespace UFAR.CoursePlan.API.SendReceiveDtos.Helpers;

public class YearSemesterObject {
    public Year year;
    public Semester semester;

    public enum Year {
        LA,
        L1,
        L2,
        L3,
        M1,
        M2
    }

    public enum Semester {
        S1,
        S2
    }
}