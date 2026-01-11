using UFAR.CoursePlan.API.SendReceiveDtos.SendDtos;

namespace UFAR.CoursePlan.API.Data.SendReceiveDtos.ReceiveDtos.Subject;

public record UpdateSubjectObject {
    public int Id { get; set; }
    public required int Number { get; set; }
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
        
    // Group dictionaries - using UpdateProfessorObject for existing subjects
    public Dictionary<int, UpdateProfessorObject>? CM_groups { get; set; }
    public Dictionary<int, UpdateProfessorObject>? CTD_groups { get; set; }
    public Dictionary<int, UpdateProfessorObject>? TD_groups { get; set; }
    public Dictionary<int, UpdateProfessorObject>? TP_groups { get; set; }
    public Dictionary<int, UpdateProfessorObject>? CTP_groups { get; set; }
    public Dictionary<int, UpdateProfessorObject>? TPS_groups { get; set; }
    public Dictionary<int, UpdateProfessorObject>? Project_groups { get; set; }
        
    // LOAP lists - using UpdateLoapItemObject for existing subjects
    public List<ValueTuple<UpdateLoapItemObject, SendSubjectObject.LoapLevel>>? Knowledge { get; set; }
    public List<ValueTuple<UpdateLoapItemObject, SendSubjectObject.LoapLevel>>? Skills { get; set; }
    public List<ValueTuple<UpdateLoapItemObject, SendSubjectObject.LoapLevel>>? SoftSkills { get; set; }
}