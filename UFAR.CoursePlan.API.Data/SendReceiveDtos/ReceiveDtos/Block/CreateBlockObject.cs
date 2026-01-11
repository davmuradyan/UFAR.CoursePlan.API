using UFAR.CoursePlan.API.Data.SendReceiveDtos.ReceiveDtos.Subject;
using UFAR.CoursePlan.API.SendReceiveDtos.Helpers;

namespace UFAR.CoursePlan.API.Data.SendReceiveDtos.ReceiveDtos;

public record CreateBlockObject {
    public required string Name { get; set; }
    public required YearSemesterObject YearSemester { get; set; }
    public List<CreateSubjectObject>? Subjects { get; set; }
}