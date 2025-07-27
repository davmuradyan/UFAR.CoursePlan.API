using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFAR.CoursePlan.API.Data.DAO;
using UFAR.CoursePlan.API.Data.Entities.Infrastructure;
using UFAR.CoursePlan.API_Core.DTOs;

namespace UFAR.CoursePlan.API_Core.Services.AdminServices {
    public class AdminServices : IAdminServices {
        private readonly MainDbContext context;
        public AdminServices(MainDbContext context) {
            this.context = context;
        }
        public async Task<bool> CreateUniversity(UniversityDto university) {
            var uni = new UniversityEntity {
                Name = university.Name
            };
            await context.Universities.AddAsync(uni);
            return await context.SaveChangesAsync().ContinueWith(t => t.Result > 0);
        }

        public async Task<bool> CreateChair(ChairDto chair) {
            var newChair = new ChairEntity {
                Name = chair.Name,
                UniversityId = (int)chair.UniversityId,
                ChairpersonId = (int)chair.ChairpersonId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await context.Chairs.AddAsync(newChair);
            return await context.SaveChangesAsync().ContinueWith(t => t.Result > 0);
        }

        public async Task<bool> CreateFaculty(FacultyDto faculty) {
            var fac = new FacultyEntity {
                Name = faculty.Name,
                UniversityId = (int)faculty.UniversityId,
                DeanId = (int)faculty.DeanId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await context.Faculties.AddAsync(fac);
            return await context.SaveChangesAsync().ContinueWith(t => t.Result > 0);
        } 
    }
}
