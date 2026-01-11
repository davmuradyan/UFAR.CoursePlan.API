namespace UFAR.CoursePlan.API.SendReceiveDtos.SendDtos;

public class MainDataAccessObject {
    public int AccountId { get; set; }

    // Main data collections using Send*Object classes to handle nulls
    public List<SendProfessorObject> Professors { get; set; } = new();
    public List<SendBlockObject> Blocks { get; set; } = new();

    // LOAP items for different categories
    public List<SendLoapItemObject> KnowledgeListLicense { get; set; } = new();
    public List<SendLoapItemObject> KnowledgeListMaster { get; set; } = new();
    public List<SendLoapItemObject> SkillListLicense { get; set; } = new();
    public List<SendLoapItemObject> SkillListMaster { get; set; } = new();
    public List<SendLoapItemObject> SoftSkillListLicense { get; set; } = new();
    public List<SendLoapItemObject> SoftSkillListMaster { get; set; } = new();
}