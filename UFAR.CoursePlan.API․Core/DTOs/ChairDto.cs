using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFAR.CoursePlan.API_Core.DTOs {
    public class ChairDto {
        public string? Name { get; set; }
        public int? UniversityId { get; set; }
        public int? ChairpersonId { get; set; } 
    }
}
