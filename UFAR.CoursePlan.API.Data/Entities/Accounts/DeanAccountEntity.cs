using UFAR.CoursePlan.API.Data.Entities.Presons;

namespace UFAR.CoursePlan.API.Data.Entities.Accounts {
    public class DeanAccountEntity : MainAbstractEntity {
        public int DeanId { get; set; }
        public DeanEntity? Dean { get; set; }
        public required string Password { get; set; }
        public required string Type { get; set; }
    }
}
