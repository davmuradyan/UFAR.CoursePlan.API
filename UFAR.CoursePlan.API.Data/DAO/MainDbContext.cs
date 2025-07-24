using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFAR.CoursePlan.API.Data.Entities.Accounts;
using UFAR.CoursePlan.API.Data.Entities.Infrastructure;
using UFAR.CoursePlan.API.Data.Entities.Presons;
using UFAR.CoursePlan.API.Data.Entities.Uni;
using UFAR.CoursePlan.API.Data.Entities.ManyToManyTables;

namespace UFAR.CoursePlan.API.Data.DAO {
    public class MainDbContext : DbContext {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }
        public DbSet<DeanAccountEntity> DeanAccounts { get; set; }
        public DbSet<DeanEntity> Deans { get; set; }
        public DbSet<ProfessorAccountEntity> ProfessorAccounts { get; set; }
        public DbSet<ProfessorEntity> Professors { get; set; }
        public DbSet<BlockEntity> Blocks { get; set; }
        public DbSet<SubjectEntity> Subjects { get; set; }
        public DbSet<LoapEntity> Loaps { get; set; }
        public DbSet<FacultyEntity> Faculties { get; set; }
        public DbSet<ChairEntity> Chairs { get; set; }
        public DbSet<UniversityEntity> Universities { get; set; }
        public DbSet<SubjectProfessorEntity> SubjectProfessors { get; set; }
        public DbSet<ChairpersonEntity> Chairpersons { get; set; }
        public DbSet<ChairpersonAccountEntity> ChairpersonAccounts { get; set; }
        public DbSet<SubjectLoapEntity> SubjectLoaps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Prevent multiple cascade paths for SubjectLoapEntity
            modelBuilder.Entity<SubjectLoapEntity>()
                .HasOne(sl => sl.Subject)
                .WithMany()
                .HasForeignKey(sl => sl.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SubjectLoapEntity>()
                .HasOne(sl => sl.Loap)
                .WithMany()
                .HasForeignKey(sl => sl.LoapId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}