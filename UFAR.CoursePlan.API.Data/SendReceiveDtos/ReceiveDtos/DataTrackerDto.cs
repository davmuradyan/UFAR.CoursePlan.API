namespace UFAR.CoursePlan.API.Data.SendReceiveDtos.ReceiveDtos;

public class DataTrackerDto {
        // Lists for tracking changes to Professor entities
        public List<CreateProfessorObject> CreateProfessorList { get; set; } = new();
        public List<UpdateProfessorObject> UpdateProfessorList { get; set; } = new();
        public List<int> DeleteProfessorList { get; set; } = new();

        // Lists for tracking changes to Block entities
        public List<CreateBlockObject> createBlockObjects { get; set; } = new();
        public List<UpdateBlockObject> updateBlockObjects { get; set; } = new();
        public List<int> deleteBlockObjects { get; set; } = new();

        // Lists for tracking changes to Knowledge License LOAP items
        public List<CreateLoapItemObject> createKnowledgeLicenseObjects { get; set; } = new();
        public List<UpdateLoapItemObject> updateKnowledgeLicenseObjects { get; set; } = new();
        public List<int> deleteKnowledgeLicenseObjects { get; set; } = new();
            
        // Lists for tracking changes to Knowledge Master LOAP items
        public List<CreateLoapItemObject> createKnowledgeMasterObjects { get; set; } = new();
        public List<UpdateLoapItemObject> updateKnowledgeMasterObjects { get; set; } = new();
        public List<int> deleteKnowledgeMasterObjects { get; set; } = new();

        // Lists for tracking changes to Skill License LOAP items
        public List<CreateLoapItemObject> createSkillLicenseObjects { get; set; } = new();
        public List<UpdateLoapItemObject> updateSkillLicenseObjects { get; set; } = new();
        public List<int> deleteSkillLicenseObjects { get; set; } = new();

        // Lists for tracking changes to Skill Master LOAP items
        public List<CreateLoapItemObject> createSkillMasterObjects { get; set; } = new();
        public List<UpdateLoapItemObject> updateSkillMasterObjects { get; set; } = new();
        public List<int> deleteSkillMasterObjects { get; set; } = new();

        // Lists for tracking changes to Soft Skill License LOAP items
        public List<CreateLoapItemObject> createSoftSkillLicenseObjects { get; set; } = new();
        public List<UpdateLoapItemObject> updateSoftSkillLicenseObjects { get; set; } = new();
        public List<int> deleteSoftSkillLicenseObjects { get; set; } = new();

        // Lists for tracking changes to Soft Skill Master LOAP items
        public List<CreateLoapItemObject> createSoftSkillMasterObjects { get; set; } = new();
        public List<UpdateLoapItemObject> updateSoftSkillMasterObjects { get; set; } = new();
        public List<int> deleteSoftSkillMasterObjects { get; set; } = new();
    }