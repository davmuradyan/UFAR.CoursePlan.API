using UFAR.CoursePlan.API.Data.DAO;
using UFAR.CoursePlan.API_Core.DTOs;
using Microsoft.IdentityModel.Tokens;
using UFAR.CoursePlan.API.Data.Entities.Presons;
using UFAR.CoursePlan.API.Data.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace UFAR.CoursePlan.API_Core.Services.DeanSide {
    public class DeanServices : IDeanServices {
        private MainDbContext context;
        public DeanServices(MainDbContext context) {
            this.context = context;
        }

        public async Task<bool> CreateDean(DeanDto dean) {
            // Validation
            if (dean.Name.IsNullOrEmpty() || dean.Surname.IsNullOrEmpty() ||
                dean.Email.IsNullOrEmpty() || dean.Password.IsNullOrEmpty()) {
                return false;
            }

            using var transaction = await context.Database.BeginTransactionAsync();
            try {
                var newDean = new DeanEntity() {
                    Name = dean.Name,
                    Surname = dean.Surname,
                    Email = dean.Email,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                await context.Deans.AddAsync(newDean);
                await context.SaveChangesAsync();

                // Hash the password before storing
                var hasher = new PasswordHasher<DeanAccountEntity>();
                var deanAccount = new DeanAccountEntity() {
                    DeanId = newDean.Id,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Password = hasher.HashPassword(null, dean.Password)
                };

                await context.DeanAccounts.AddAsync(deanAccount);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            } catch (Exception ex) {
                await transaction.RollbackAsync();

                Console.WriteLine("[ERROR]\tDeanService: Transaction failed!");
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        public async Task<bool> CreateProfessor(ProfessorDto professor, int deanId) {
            // Validation
            if (professor.Name.IsNullOrEmpty() || professor.Surname.IsNullOrEmpty() ||
                professor.Email.IsNullOrEmpty() || !professor.Password.IsNullOrEmpty()) {
                return false;
            }

            // Corrected code to retrieve facultyId
            var faculty = await context.Faculties
                .FirstOrDefaultAsync(f => f.DeanId == deanId);

            if (faculty == null) {
                Console.WriteLine("[ERROR]\tDeanService: Faculty not found for the given Dean ID.");
                return false;
            }

            int facultyId = faculty.Id;

            var newProfessor = new ProfessorEntity() {
                Name = professor.Name,
                Surname = professor.Surname,
                Email = professor.Email,
                Phone = professor.Phone,
                FacultyId = facultyId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            try {
                await context.Professors.AddAsync(newProfessor);
                await context.SaveChangesAsync();
                return true;
            } catch (Exception ex) {
                Console.WriteLine("[ERROR]\tDeanService: Failed to create professor.");
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<LoginResult> TryLoginFromDean(DeanLoginDto loginData) {
            try {
                var dean = await context.Deans.FirstOrDefaultAsync(d => d.Email == loginData.Email);
                if (dean == null) {
                    Console.WriteLine("[InvalidLogin]\tDeanService: Dean not found with the provided email.");
                    return LoginResult.InvalidCredentials;
                }

                var deanAccount = await context.DeanAccounts.FirstOrDefaultAsync(da => da.DeanId == dean.Id);
                if (deanAccount == null) {
                    Console.WriteLine("[InvalidLogin]\tDeanService: Dean account not found.");
                    return LoginResult.InvalidCredentials;
                }

                var hasher = new PasswordHasher<DeanAccountEntity>();
                var result = hasher.VerifyHashedPassword(deanAccount, deanAccount.Password, loginData.Password);

                if (result == PasswordVerificationResult.Failed) {
                    Console.WriteLine("[InvalidLogin]\tDeanService: Invalid password.");
                    return LoginResult.InvalidCredentials;
                }

                return LoginResult.Success;
            } catch (Exception ex) {
                Console.WriteLine($"[ERROR] Error occurred while login attempt with email: {loginData.Email}");
                return LoginResult.Error;
            }
        }
    }
}
