using UFAR.CoursePlan.API.SendReceiveDtos.Helpers;

namespace UFAR.CoursePlan.API.SendReceiveDtos.SendDtos;

public record SendBlockObject {
    public int Id { get; set; }
    public string? Name { get; set; }
    public YearSemesterObject YearSemester { get; set; } = null!;
    public List<SendSubjectObject>? Subjects { get; set; }
}