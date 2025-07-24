using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFAR.CoursePlan.API.Data.Entities.Infrastructure {
    public abstract class AbstractInfrastructure : MainAbstractEntity {
        public required string Name { get; set; }
        public int UniversityId { get; set; }
        public UniversityEntity? University { get; set; }
    }
}
