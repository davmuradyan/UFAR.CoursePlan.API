using Microsoft.AspNetCore.Mvc;
using UFAR.CoursePlan.API.Data.DAO;
using UFAR.CoursePlan.API.Data.Entities.ManyToManyTables;
using UFAR.CoursePlan.API.Data.Entities.Presons;
using UFAR.CoursePlan.API.Data.Entities.Uni;
using UFAR.CoursePlan.API.SendReceiveDtos.Helpers;
using UFAR.CoursePlan.API.SendReceiveDtos.SendDtos;
using UFAR.CoursePlan.API_Core.DTOs;
using UFAR.CoursePlan.API_Core.Services.ChairpersonSide;
using UFAR.CoursePlan.API_Core.Services.DeanSide;


namespace UFAR.CoursePlan.API.Controllers {
    public class LoginController : Controller {
        IDeanServices deanServices;
        IChairpersonSide chairpersonSide;
        MainDbContext mainDbContext;
        public LoginController(IDeanServices deanServices, IChairpersonSide chairpersonSide, MainDbContext mainDbContext)
        {
            this.deanServices = deanServices;
            this.chairpersonSide = chairpersonSide;
            this.mainDbContext = mainDbContext;
        }

        [HttpPost("CreateDean")]
        public async Task<IActionResult> CreateDean([FromBody] DeanDto dean) {
            if (await deanServices.CreateDean(dean)) {
                return Ok("Dean created successfully");
            } else {
                return BadRequest("Failed to create dean");
            }
        }

        [HttpPost("TryLoginFromDean")]
        public async Task<IActionResult> TryLoginFromDean([FromBody] DeanLoginDto loginData)
        {
            var result = await deanServices.TryLoginFromDean(loginData);
            if (result > 0)
            {
                MainDataAccessObject obj = new MainDataAccessObject();
                obj.AccountId = result;

                var faculty = mainDbContext.Faculties.FirstOrDefault(f => f.DeanId == result);
                if (faculty != null)
                {
                    PrepareDataAccessObject(obj, faculty.Id);
                }

                return Ok(obj);
            }
            else if ((LoginResult)(-result) == LoginResult.InvalidCredentials)
            {
                return Unauthorized("Invalid credentials");
            }
            else
            {
                return StatusCode(500, "An error occurred during login");
            }
        }
        [HttpPost("CreateChairperson")]
        public async Task<IActionResult> CreateChairperson([FromBody] ChairpersonDto chairperson) {
            if (await chairpersonSide.CreateChairperson(chairperson)) {
                return Ok("Chairperson created successfully");
            } else {
                return BadRequest("Failed to create chairperson");
            }
        }

        void PrepareDataAccessObject(MainDataAccessObject obj, int fId) { 
            var loap = mainDbContext.Loaps.Where(l => l.FacultyId == fId).ToList();
            var profs = mainDbContext.Professors.Where(p => p.FacultyId == fId).ToList();
            var blocks = mainDbContext.Blocks.Where(b => b.FacultyId == fId).ToList();
            var subjects = mainDbContext.Subjects
                .Where(s => blocks.Select(b => b.Id).Contains(s.BlockId))
                .ToList();
            var subjLoap = mainDbContext.SubjectLoaps
                .Where(sl => subjects.Select(s => s.Id).Contains(sl.SubjectId))
                .ToList();
            var subjProf = mainDbContext.SubjectProfessors
                .Where(sp => subjects.Select(s => s.Id).Contains(sp.SubjectId))
                .ToList();

            // Populate Professors list
            obj.Professors = profs.Select(p => new SendProfessorObject
            {
                Id = p.Id,
                Name = p.Name,
                Surname = p.Surname,
                Email = p.Email,
                PhoneNumber = p.Phone
            }).ToList();

            // knowledge
            obj.KnowledgeListLicense = loap.Where(l => l.LoapType == LoapType.Knowledge)
                .Where(l => l.Degree.ToLower() == "license")
                .Select(l => new SendLoapItemObject {
                    Id = l.Id,
                    Value = l.Value,
                })
                .ToList();
            obj.KnowledgeListMaster = loap.Where(l => l.LoapType == LoapType.Knowledge)
                .Where(l => l.Degree.ToLower() == "master")
                .Select(l => new SendLoapItemObject {
                    Id = l.Id,
                    Value = l.Value,
                })
                .ToList();

            // Skill
            obj.SkillListLicense = loap.Where(l => l.LoapType == LoapType.Skill)
                .Where(l => l.Degree.ToLower() == "license")
                .Select(l => new SendLoapItemObject {
                    Id = l.Id,
                    Value = l.Value,
                })
                .ToList();
            obj.SkillListMaster = loap.Where(l => l.LoapType == LoapType.Skill)
                .Where(l => l.Degree.ToLower() == "master")
                .Select(l => new SendLoapItemObject {
                    Id = l.Id,
                    Value = l.Value,
                })
                .ToList();

            // Soft Skill
            obj.SoftSkillListLicense = loap.Where(l => l.LoapType == LoapType.SoftSkill)
                .Where(l => l.Degree.ToLower() == "license")
                .Select(l => new SendLoapItemObject {
                    Id = l.Id,
                    Value = l.Value,
                })
                .ToList();
            obj.SoftSkillListMaster = loap.Where(l => l.LoapType == LoapType.SoftSkill)
                .Where(l => l.Degree.ToLower() == "master")
                .Select(l => new SendLoapItemObject {
                    Id = l.Id,
                    Value = l.Value,
                })
                .ToList();

            obj.Blocks = blocks
                .Select(b => new SendBlockObject {
                    Id = b.Id,
                    Name = "Bloc " + b.Number,
                    YearSemester = new YearSemesterObject {
                        year = (YearSemesterObject.Year)b.Year,
                        semester = (YearSemesterObject.Semester)b.Semester
                    },
                    Subjects = subjects.Where(sub => sub.BlockId == b.Id).Select(sub => {
                        var s = new SendSubjectObject { Id = sub.Id };
                        PrepareSubject(s, sub);
                        AppendLoapsToSubject(s, subjLoap, obj);
                        AppendProfessorsToSubject(s, subjProf, profs);
                        return s;
                    }).ToList()
                })
                .ToList();
        }

        void PrepareSubject(SendSubjectObject s, SubjectEntity entity) {
            s.Name = entity.Name;
            s.ECTS = entity.ECTS;
            s.Project = entity.Project;
            s.CTP = entity.CTP;
            s.TP = entity.TP;
            s.TPS = entity.TPS;
            s.TD = entity.TD;
            s.CTD = entity.CTD;
            s.CM = entity.CM;
        }
        void AppendLoapsToSubject(SendSubjectObject s, List<SubjectLoapEntity> subjLoap, MainDataAccessObject obj) {
            var subjectLoaps = subjLoap.Where(sl => sl.SubjectId == s.Id).ToList();
            
            s.Knowledge = new List<ValueTuple<SendLoapItemObject, SendSubjectObject.LoapLevel>>();
            s.Skills = new List<ValueTuple<SendLoapItemObject, SendSubjectObject.LoapLevel>>();
            s.SoftSkills = new List<ValueTuple<SendLoapItemObject, SendSubjectObject.LoapLevel>>();

            foreach (var sl in subjectLoaps) {
                var loapItem = obj.KnowledgeListLicense.FirstOrDefault(l => l.Id == sl.LoapId);
                if (loapItem == null) loapItem = obj.KnowledgeListMaster.FirstOrDefault(l => l.Id == sl.LoapId);
                
                if (loapItem != null) {
                    s.Knowledge.Add((loapItem, (SendSubjectObject.LoapLevel)sl.Level));
                    continue;
                }

                loapItem = obj.SkillListLicense.FirstOrDefault(l => l.Id == sl.LoapId);
                if (loapItem == null) loapItem = obj.SkillListMaster.FirstOrDefault(l => l.Id == sl.LoapId);
                
                if (loapItem != null) {
                    s.Skills.Add((loapItem, (SendSubjectObject.LoapLevel)sl.Level));
                    continue;
                }

                loapItem = obj.SoftSkillListLicense.FirstOrDefault(l => l.Id == sl.LoapId);
                if (loapItem == null) loapItem = obj.SoftSkillListMaster.FirstOrDefault(l => l.Id == sl.LoapId);
                
                if (loapItem != null) {
                    s.SoftSkills.Add((loapItem, (SendSubjectObject.LoapLevel)sl.Level));
                }
            }
        }
        void AppendProfessorsToSubject(SendSubjectObject s, List<SubjectProfessorEntity> subjProf, List<ProfessorEntity> profs)
        {
            var subjectProfessors = subjProf.Where(sp => sp.SubjectId == s.Id).ToList();

            s.CM_groups = new Dictionary<int, SendProfessorObject>();
            s.CTD_groups = new Dictionary<int, SendProfessorObject>();
            s.TD_groups = new Dictionary<int, SendProfessorObject>();
            s.TP_groups = new Dictionary<int, SendProfessorObject>();
            s.CTP_groups = new Dictionary<int, SendProfessorObject>();
            s.TPS_groups = new Dictionary<int, SendProfessorObject>();
            s.Project_groups = new Dictionary<int, SendProfessorObject>();

            foreach (var sp in subjectProfessors)
            {
                if (sp.ProfessorId.HasValue)
                {
                    var professor = profs.FirstOrDefault(p => p.Id == sp.ProfessorId.Value);
                    if (professor != null)
                    {
                        var sendProf = new SendProfessorObject
                        {
                            Id = professor.Id,
                            Name = professor.Name,
                            Surname = professor.Surname,
                            Email = professor.Email,
                            PhoneNumber = professor.Phone
                        };

                        switch (sp.GroupType)
                        {
                            case GroupType.CM:
                                s.CM_groups[sp.GroupNumber] = sendProf;
                                break;
                            case GroupType.CTD:
                                s.CTD_groups[sp.GroupNumber] = sendProf;
                                break;
                            case GroupType.TD:
                                s.TD_groups[sp.GroupNumber] = sendProf;
                                break;
                            case GroupType.TP:
                                s.TP_groups[sp.GroupNumber] = sendProf;
                                break;
                            case GroupType.CTP:
                                s.CTP_groups[sp.GroupNumber] = sendProf;
                                break;
                            case GroupType.TPS:
                                s.TPS_groups[sp.GroupNumber] = sendProf;
                                break;
                            case GroupType.Project:
                                s.Project_groups[sp.GroupNumber] = sendProf;
                                break;
                        }
                    }
                }
            }
        }
    }
}