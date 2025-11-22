using GymMangementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementDAL.Data.Contexts
{
    public class GymDbContext: IdentityDbContext<ApplicationUser>
    {
        public GymDbContext(DbContextOptions<GymDbContext>options):base(options)
        {
            
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server = .; Database = GymManagementDB; Trusted_Connection = True; TrustServerCertificate=True;");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<ApplicationUser>(Eb =>
            {
                Eb.Property(u => u.FirstName)
                    .HasColumnType("varchar")
                    .HasMaxLength(50);

                Eb.Property(u => u.LastName)
                     .HasColumnType("varchar")
                     .HasMaxLength(50);
            });
        }
        #region Dbset
        // public DbSet<ApplicationUser> Users { get; set; }
        //public DbSet<IdentityRole> Roles { get; set; }
        //public DbSet<IdentityUserRole<string>> UserRoles { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<MemberSession> MemberSessions { get; set; }
        public DbSet<Plan> Plans { get; set; }

        #endregion
    }
}
