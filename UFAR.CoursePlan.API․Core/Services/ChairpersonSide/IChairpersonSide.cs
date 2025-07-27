using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFAR.CoursePlan.API_Core.DTOs;

namespace UFAR.CoursePlan.API_Core.Services.ChairpersonSide {
    public interface IChairpersonSide {
        public Task<bool> CreateChairperson(ChairpersonDto chairperson);
    }
}
