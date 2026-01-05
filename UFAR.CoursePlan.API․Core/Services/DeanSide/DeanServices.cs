using UFAR.CoursePlan.API.Data.DAO;
using UFAR.CoursePlan.API_Core.DTOs;
using Microsoft.IdentityModel.Tokens;
using UFAR.CoursePlan.API.Data.Entities.Presons;
using UFAR.CoursePlan.API.Data.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using UFAR.CoursePlan.API_Core.CreatingDtos;

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

        public async Task<bool> CreateProfessors(int deanId, List<CreateProfDto> professors) {
            try {

                int facultyId = await context.Faculties
                    .Where(f => f.DeanId == deanId)
                    .Select(f => f.Id)
                    .FirstOrDefaultAsync();
                professors = professors.Where(p => !(p.Name.IsNullOrEmpty() || p.Surname.IsNullOrEmpty()
                                                 || p.Email.IsNullOrEmpty() || p.Phone.IsNullOrEmpty())).ToList();
                foreach (var prof in professors) {
                    var alreadyExists = await context.Professors
                        .AnyAsync(p => p.Email == prof.Email && p.FacultyId == facultyId &&
                                        p.Name == prof.Name && p.Surname == p.Surname);
                    if (!alreadyExists) {
                        var transaction = await context.Database.BeginTransactionAsync();
                        var newProf = new ProfessorEntity() {
                            Name = prof.Name,
                            Surname = prof.Surname,
                            Email = prof.Email,
                            Phone = prof.Phone,
                            FacultyId = facultyId,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                        await context.Professors.AddAsync(newProf);
                        await context.SaveChangesAsync();

                        var profAccount = new ProfessorAccountEntity() {
                            ProfessorId = newProf.Id,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            Password = new PasswordHasher<ProfessorAccountEntity>()
                                .HashPassword(null, "changeYourPassword") // Assuming prof.Password is provided
                        };

                        await context.ProfessorAccounts.AddAsync(profAccount);
                        await context.SaveChangesAsync();
                        transaction.Commit();
                    }
                }
                return true;
            } catch {
                return false;
            }
        }

        public async Task<int> TryLoginFromDean(DeanLoginDto loginData) {
            try {
                var dean = await context.Deans.FirstOrDefaultAsync(d => d.Email == loginData.Email);
                if (dean == null) {
                    Console.WriteLine("[InvalidLogin]\tDeanService: Dean not found with the provided email.");
                    return -(int)LoginResult.InvalidCredentials;
                }

                var deanAccount = await context.DeanAccounts.FirstOrDefaultAsync(da => da.DeanId == dean.Id);
                if (deanAccount == null) {
                    Console.WriteLine("[InvalidLogin]\tDeanService: Dean account not found.");
                    return -(int)LoginResult.InvalidCredentials;
                }

                var hasher = new PasswordHasher<DeanAccountEntity>();
                var result = hasher.VerifyHashedPassword(deanAccount, deanAccount.Password, loginData.Password);

                if (result == PasswordVerificationResult.Failed) {
                    Console.WriteLine("[InvalidLogin]\tDeanService: Invalid password.");
                    return -(int)LoginResult.InvalidCredentials;
                }

                return dean.Id;
            } catch (Exception ex) {
                Console.WriteLine($"[ERROR] Error occurred while login attempt with email: {loginData.Email}");
                return -(int)LoginResult.Error;
            }
        }

        public async Task<List<ProfessorEntity>> GetProfessors(int deanId) {
            try {
                int facultyId = await context.Faculties
                    .Where(f => f.DeanId == deanId)
                    .Select(f => f.Id)
                    .FirstOrDefaultAsync();
                return await context.Professors
                    .Where(p => p.FacultyId == facultyId)
                    .ToListAsync();
            } catch (Exception ex) {
                Console.WriteLine($"[ERROR] Error occurred while fetching professors for dean ID: {deanId}");
                Console.WriteLine(ex.Message);
                return new List<ProfessorEntity>();
            }
        }
    }
}