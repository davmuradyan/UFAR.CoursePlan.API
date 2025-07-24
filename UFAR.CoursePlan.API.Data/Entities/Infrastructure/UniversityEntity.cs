using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFAR.CoursePlan.API.Data.Entities.Infrastructure {
    public class UniversityEntity : MainAbstractEntity {
        public required string Name { get; set; }
    }
}
