using UFAR.CoursePlan.API.Data.DAO;
using UFAR.CoursePlan.API_Core.DTOs;
using Microsoft.IdentityModel.Tokens;
using UFAR.CoursePlan.API.Data.Entities.Presons;
using UFAR.CoursePlan.API.Data.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using UFAR.CoursePlan.API_Core.CreatingDtos;
using UFAR.CoursePlan.API.Data.SendReceiveDtos.ReceiveDtos;
using UFAR.CoursePlan.API.Data.Entities.Uni;

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

            var strategy = context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync<MainDbContext, bool>(context, async (ctx, ct) => {
                using var transaction = await ctx.Database.BeginTransactionAsync(ct);
                try {
                    var newDean = new DeanEntity() {
                        Name = dean.Name,
                        Surname = dean.Surname,
                        Email = dean.Email,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    };

                    await ctx.Deans.AddAsync(newDean, ct);
                    await ctx.SaveChangesAsync(ct);

                    // Hash the password before storing
                    var hasher = new PasswordHasher<DeanAccountEntity>();
                    var deanAccount = new DeanAccountEntity() {
                        DeanId = newDean.Id,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        Password = hasher.HashPassword(null, dean.Password)
                    };

                    await ctx.DeanAccounts.AddAsync(deanAccount, ct);
                    await ctx.SaveChangesAsync(ct);

                    await transaction.CommitAsync(ct);
                    return true;
                } catch (Exception ex) {
                    await transaction.RollbackAsync(ct);

                    Console.WriteLine("[ERROR]\tDeanService: Transaction failed!");
                    Console.WriteLine(ex.Message);

                    return false;
                }
            }, null, default);
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
                        var strategy = context.Database.CreateExecutionStrategy();
                        await strategy.ExecuteAsync<MainDbContext, bool>(context, async (ctx, ct) => {
                            using var transaction = await ctx.Database.BeginTransactionAsync(ct);
                            try {
                                var newProf = new ProfessorEntity() {
                                    Name = prof.Name,
                                    Surname = prof.Surname,
                                    Email = prof.Email,
                                    Phone = prof.Phone,
                                    FacultyId = facultyId,
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now
                                };
                                await ctx.Professors.AddAsync(newProf, ct);
                                await ctx.SaveChangesAsync(ct);

                                var profAccount = new ProfessorAccountEntity() {
                                    ProfessorId = newProf.Id,
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now,
                                    Password = new PasswordHasher<ProfessorAccountEntity>()
                                        .HashPassword(null, "changeYourPassword") // Assuming prof.Password is provided
                                };

                                await ctx.ProfessorAccounts.AddAsync(profAccount, ct);
                                await ctx.SaveChangesAsync(ct);
                                await transaction.CommitAsync(ct);
                                return true;
                            } catch {
                                await transaction.RollbackAsync(ct);
                                throw;
                            }
                        }, null, default);
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

        public async Task<bool> ApplyDataTrackerChanges(int deanId, DataTrackerDto dataTrackerDto) {
            var strategy = context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync<MainDbContext, bool>(context, async (ctx, ct) => {
                using var transaction = await ctx.Database.BeginTransactionAsync(ct);
                try {
                    // Get dean's faculty ID
                    int facultyId = await ctx.Faculties
                        .Where(f => f.DeanId == deanId)
                        .Select(f => f.Id)
                        .FirstOrDefaultAsync(ct);

                    if (facultyId == 0) {
                        Console.WriteLine("[ERROR]\tDeanService: Faculty not found for dean.");
                        return false;
                    }

                // 1. Handle Professor Deletes
                if (dataTrackerDto.DeleteProfessorList != null && dataTrackerDto.DeleteProfessorList.Count > 0) {
                    var professorIds = dataTrackerDto.DeleteProfessorList;
                    var professorsToDelete = await ctx.Professors
                        .Where(p => professorIds.Contains(p.Id))
                        .ToListAsync(ct);
                    ctx.Professors.RemoveRange(professorsToDelete);
                }

                // 2. Handle Professor Updates
                if (dataTrackerDto.UpdateProfessorList != null && dataTrackerDto.UpdateProfessorList.Count > 0) {
                    foreach (var profUpdate in dataTrackerDto.UpdateProfessorList) {
                        var professor = await ctx.Professors.FindAsync(new object[] { profUpdate.Id }, ct);
                        if (professor != null) {
                            if (!string.IsNullOrEmpty(profUpdate.Name))
                                professor.Name = profUpdate.Name;
                            if (!string.IsNullOrEmpty(profUpdate.Surname))
                                professor.Surname = profUpdate.Surname;
                            if (!string.IsNullOrEmpty(profUpdate.Email))
                                professor.Email = profUpdate.Email;
                            if (!string.IsNullOrEmpty(profUpdate.PhoneNumber))
                                professor.Phone = profUpdate.PhoneNumber;
                            professor.UpdatedAt = DateTime.Now;
                        }
                    }
                }

                // 3. Handle Professor Creates
                if (dataTrackerDto.CreateProfessorList != null && dataTrackerDto.CreateProfessorList.Count > 0) {
                    foreach (var profCreate in dataTrackerDto.CreateProfessorList) {
                        var newProfessor = new ProfessorEntity {
                            Name = profCreate.Name,
                            Surname = profCreate.Surname,
                            Email = profCreate.Email,
                            Phone = profCreate.PhoneNumber,
                            FacultyId = facultyId,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                        await ctx.Professors.AddAsync(newProfessor, ct);
                    }
                }

                // 4. Handle Block Deletes (and cascade delete subjects)
                if (dataTrackerDto.deleteBlockObjects != null && dataTrackerDto.deleteBlockObjects.Count > 0) {
                    var blockIds = dataTrackerDto.deleteBlockObjects;
                    
                    // Delete subjects in these blocks first
                    var subjectsToDelete = await ctx.Subjects
                        .Where(s => blockIds.Contains(s.BlockId))
                        .ToListAsync(ct);
                    ctx.Subjects.RemoveRange(subjectsToDelete);
                    
                    // Then delete blocks
                    var blocksToDelete = await ctx.Blocks
                        .Where(b => blockIds.Contains(b.Id))
                        .ToListAsync(ct);
                    ctx.Blocks.RemoveRange(blocksToDelete);
                }

                // 5. Handle Block Updates
                if (dataTrackerDto.updateBlockObjects != null && dataTrackerDto.updateBlockObjects.Count > 0) {
                    foreach (var blockUpdate in dataTrackerDto.updateBlockObjects) {
                        var block = await ctx.Blocks.FindAsync(new object[] { blockUpdate.Id }, ct);
                        if (block != null) {
                            if (blockUpdate.YearSemester != null) {
                                block.Year = (Year)blockUpdate.YearSemester.year;
                                block.Semester = (Semester)blockUpdate.YearSemester.semester;
                            }
                            block.UpdatedAt = DateTime.Now;

                            // Handle subjects in the updated block
                            if (blockUpdate.Subjects != null && blockUpdate.Subjects.Count > 0) {
                                foreach (var subjectUpdate in blockUpdate.Subjects) {
                                    var subject = await ctx.Subjects.FindAsync(new object[] { subjectUpdate.Id }, ct);
                                    if (subject != null) {
                                        if (!string.IsNullOrEmpty(subjectUpdate.Name))
                                            subject.Name = subjectUpdate.Name;
                                        if (!string.IsNullOrEmpty(subjectUpdate.UE))
                                            subject.UE = subjectUpdate.UE;
                                        if (subjectUpdate.CM.HasValue)
                                            subject.CM = subjectUpdate.CM.Value;
                                        if (subjectUpdate.TD.HasValue)
                                            subject.TD = subjectUpdate.TD.Value;
                                        if (subjectUpdate.CTD.HasValue)
                                            subject.CTD = subjectUpdate.CTD.Value;
                                        if (subjectUpdate.TP.HasValue)
                                            subject.TP = subjectUpdate.TP.Value;
                                        if (subjectUpdate.CTP.HasValue)
                                            subject.CTP = subjectUpdate.CTP.Value;
                                        if (subjectUpdate.TPS.HasValue)
                                            subject.TPS = subjectUpdate.TPS.Value;
                                        if (subjectUpdate.Project.HasValue)
                                            subject.Project = subjectUpdate.Project.Value;
                                        if (subjectUpdate.ECTS.HasValue)
                                            subject.ECTS = subjectUpdate.ECTS.Value;
                                        subject.UpdatedAt = DateTime.Now;
                                    }
                                }
                            }
                        }
                    }
                }

                // 6. Handle Block Creates
                if (dataTrackerDto.createBlockObjects != null && dataTrackerDto.createBlockObjects.Count > 0) {
                    foreach (var blockCreate in dataTrackerDto.createBlockObjects) {
                        var newBlock = new BlockEntity {
                            Number = 0,
                            Year = (Year)(int)blockCreate.YearSemester.year,
                            Semester = (Semester)(int)blockCreate.YearSemester.semester,
                            FacultyId = facultyId,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                        await ctx.Blocks.AddAsync(newBlock, ct);
                        await ctx.SaveChangesAsync(ct); // Save to get block ID

                        // Create subjects for this block
                        if (blockCreate.Subjects != null && blockCreate.Subjects.Count > 0) {
                            foreach (var subjectCreate in blockCreate.Subjects) {
                                var newSubject = new SubjectEntity {
                                    Name = subjectCreate.Name,
                                    UE = subjectCreate.UE,
                                    CM = subjectCreate.CM,
                                    TD = subjectCreate.TD,
                                    CTD = subjectCreate.CTD,
                                    TP = subjectCreate.TP,
                                    CTP = subjectCreate.CTP,
                                    TPS = subjectCreate.TPS,
                                    Project = subjectCreate.Project,
                                    ECTS = subjectCreate.ECTS,
                                    BlockId = newBlock.Id,
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now
                                };
                                await ctx.Subjects.AddAsync(newSubject, ct);
                            }
                        }
                    }
                }

                // 7. Handle LOAP Deletes - collect all delete IDs
                var allLoapDeleteIds = new List<int>();
                if (dataTrackerDto.deleteKnowledgeLicenseObjects != null)
                    allLoapDeleteIds.AddRange(dataTrackerDto.deleteKnowledgeLicenseObjects);
                if (dataTrackerDto.deleteKnowledgeMasterObjects != null)
                    allLoapDeleteIds.AddRange(dataTrackerDto.deleteKnowledgeMasterObjects);
                if (dataTrackerDto.deleteSkillLicenseObjects != null)
                    allLoapDeleteIds.AddRange(dataTrackerDto.deleteSkillLicenseObjects);
                if (dataTrackerDto.deleteSkillMasterObjects != null)
                    allLoapDeleteIds.AddRange(dataTrackerDto.deleteSkillMasterObjects);
                if (dataTrackerDto.deleteSoftSkillLicenseObjects != null)
                    allLoapDeleteIds.AddRange(dataTrackerDto.deleteSoftSkillLicenseObjects);
                if (dataTrackerDto.deleteSoftSkillMasterObjects != null)
                    allLoapDeleteIds.AddRange(dataTrackerDto.deleteSoftSkillMasterObjects);

                if (allLoapDeleteIds.Count > 0) {
                    var loapsToDelete = await ctx.Loaps
                        .Where(l => allLoapDeleteIds.Contains(l.Id))
                        .ToListAsync(ct);
                    ctx.Loaps.RemoveRange(loapsToDelete);
                }

                // 8. Handle LOAP Updates
                await HandleLoapUpdates(dataTrackerDto.updateKnowledgeLicenseObjects, "License", LoapType.Knowledge);
                await HandleLoapUpdates(dataTrackerDto.updateSkillLicenseObjects, "License", LoapType.Skill);
                await HandleLoapUpdates(dataTrackerDto.updateSoftSkillLicenseObjects, "License", LoapType.SoftSkill);
                await HandleLoapUpdates(dataTrackerDto.updateKnowledgeMasterObjects, "Master", LoapType.Knowledge);
                await HandleLoapUpdates(dataTrackerDto.updateSkillMasterObjects, "Master", LoapType.Skill);
                await HandleLoapUpdates(dataTrackerDto.updateSoftSkillMasterObjects, "Master", LoapType.SoftSkill);

                // 9. Handle LOAP Creates
                await HandleLoapCreates(dataTrackerDto.createKnowledgeLicenseObjects, "License", LoapType.Knowledge, facultyId);
                await HandleLoapCreates(dataTrackerDto.createSkillLicenseObjects, "License", LoapType.Skill, facultyId);
                await HandleLoapCreates(dataTrackerDto.createSoftSkillLicenseObjects, "License", LoapType.SoftSkill, facultyId);
                await HandleLoapCreates(dataTrackerDto.createKnowledgeMasterObjects, "Master", LoapType.Knowledge, facultyId);
                await HandleLoapCreates(dataTrackerDto.createSkillMasterObjects, "Master", LoapType.Skill, facultyId);
                await HandleLoapCreates(dataTrackerDto.createSoftSkillMasterObjects, "Master", LoapType.SoftSkill, facultyId);

                await ctx.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
                return true;
            } catch (Exception ex) {
                await transaction.RollbackAsync(ct);
                Console.WriteLine("[ERROR]\tDeanService: ApplyDataTrackerChanges transaction failed!");
                Console.WriteLine(ex.Message);
                return false;
            }
            }, null, default);
        }

        private async Task HandleLoapUpdates(List<UpdateLoapItemObject>? loapItems, string degree, LoapType loapType) {
            if (loapItems == null || loapItems.Count == 0) return;

            foreach (var loapUpdate in loapItems) {
                var loap = await context.Loaps.FindAsync(loapUpdate.Id);
                if (loap != null) {
                    if (!string.IsNullOrEmpty(loapUpdate.Value))
                        loap.Value = loapUpdate.Value;
                    loap.UpdatedAt = DateTime.Now;
                }
            }
        }

        private async Task HandleLoapCreates(List<CreateLoapItemObject>? loapItems, string degree, LoapType loapType, int facultyId) {
            if (loapItems == null || loapItems.Count == 0) return;

            foreach (var loapCreate in loapItems) {
                var newLoap = new LoapEntity {
                    Value = loapCreate.Value,
                    Degree = degree,
                    LoapType = loapType,
                    FacultyId = facultyId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                await context.Loaps.AddAsync(newLoap);
            }
        }
    }
}