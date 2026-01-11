using UFAR.CoursePlan.API.Data.SendReceiveDtos.ReceiveDtos.Subject;
using UFAR.CoursePlan.API.SendReceiveDtos.Helpers;

namespace UFAR.CoursePlan.API.Data.SendReceiveDtos.ReceiveDtos;

public record UpdateBlockObject {
    public int Id { get; set; }
    public string? Name { get; set; }
    public YearSemesterObject? YearSemester { get; set; }
    public List<UpdateSubjectObject>? Subjects { get; set; }
}