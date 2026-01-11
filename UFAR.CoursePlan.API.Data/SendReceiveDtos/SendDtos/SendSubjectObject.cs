namespace UFAR.CoursePlan.API.SendReceiveDtos.SendDtos;

public record SendSubjectObject {
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? UE { get; set; }
    public double? CM { get; set; }
    public double? TD { get; set; }
    public double? CTD { get; set; }
    public double? TP { get; set; }
    public double? CTP { get; set; }
    public double? TPS { get; set; }
    public double? Project { get; set; }
    public double? ECTS { get; set; }

    // Group dictionaries - using ReceivedProfessorObject for existing subjects
    public Dictionary<int, SendProfessorObject>? CM_groups { get; set; }
    public Dictionary<int, SendProfessorObject>? CTD_groups { get; set; }
    public Dictionary<int, SendProfessorObject>? TD_groups { get; set; }
    public Dictionary<int, SendProfessorObject>? TP_groups { get; set; }
    public Dictionary<int, SendProfessorObject>? CTP_groups { get; set; }
    public Dictionary<int, SendProfessorObject>? TPS_groups { get; set; }
    public Dictionary<int, SendProfessorObject>? Project_groups { get; set; }

    // LOAP lists - using ReceivedLoapItemObject for existing subjects
    public List<ValueTuple<SendLoapItemObject, LoapLevel>>? Knowledge { get; set; }
    public List<ValueTuple<SendLoapItemObject, LoapLevel>>? Skills { get; set; }
    public List<ValueTuple<SendLoapItemObject, LoapLevel>>? SoftSkills { get; set; }

    public enum LoapLevel {
        L1,
        L2,
        L3
    };
}