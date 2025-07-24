using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFAR.CoursePlan.API.Data.Entities.Presons;

namespace UFAR.CoursePlan.API.Data.Entities.Accounts {
    public class ChairpersonAccountEntity : AbstractAccount {
        public int ChairpersonId { get; set; }
        public ChairpersonEntity? Chairperson { get; set; }
    }
}