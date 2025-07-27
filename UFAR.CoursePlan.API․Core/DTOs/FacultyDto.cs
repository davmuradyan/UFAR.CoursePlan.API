using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFAR.CoursePlan.API_Core.DTOs {
    public class FacultyDto {
        public string? Name { get; set; }
        public int? UniversityId { get; set; }
        public int? DeanId { get; set; }
    }
}
