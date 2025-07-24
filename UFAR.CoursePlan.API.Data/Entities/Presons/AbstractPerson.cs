using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFAR.CoursePlan.API.Data.Entities.Presons {
    public abstract class AbstractPerson : MainAbstractEntity {
        public required string Name { get; init; }
        public required string Surname { get; init; }
        public required string Email { get; init; }
    }
}
