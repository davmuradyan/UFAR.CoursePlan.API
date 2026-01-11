using UFAR.CoursePlan.API.SendReceiveDtos.SendDtos;

namespace UFAR.CoursePlan.API.Data.SendReceiveDtos.ReceiveDtos.Subject;

public record CreateSubjectObject {
    public required string Name { get; init; }
    public required string UE { get; init; }
    public required double CM { get; init; }
    public required double TD { get; init; }
    public required double CTD { get; init; }
    public required double TP { get; init; }
    public required double CTP { get; init; }
    public required double TPS { get; init; }
    public required double Project { get; init; }
    public required double ECTS { get; init; }

    // Group dictionaries - using CreateProfessorObject for new subjects
    public Dictionary<int, CreateProfessorObject>? CM_groups { get; init; }
    public Dictionary<int, CreateProfessorObject>? CTD_groups { get; init; }
    public Dictionary<int, CreateProfessorObject>? TD_groups { get; init; }
    public Dictionary<int, CreateProfessorObject>? TP_groups { get; init; }
    public Dictionary<int, CreateProfessorObject>? CTP_groups { get; init; }
    public Dictionary<int, CreateProfessorObject>? TPS_groups { get; init; }
    public Dictionary<int, CreateProfessorObject>? Project_groups { get; init; }

    // LOAP lists - using CreateLoapItemObject for new subjects
    public List<ValueTuple<CreateLoapItemObject, SendSubjectObject.LoapLevel>>? Knowledge { get; init; }
    public List<ValueTuple<CreateLoapItemObject, SendSubjectObject.LoapLevel>>? Skills { get; init; }
    public List<ValueTuple<CreateLoapItemObject, SendSubjectObject.LoapLevel>>? SoftSkills { get; init; }
}