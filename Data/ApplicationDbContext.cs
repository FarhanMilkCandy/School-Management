using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SMS.Models;

namespace SMS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Configuration> Configurations { get; set; }
        public virtual DbSet<Admission> Admissions { get; set; }
        public virtual DbSet<Institute> Institutes { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Teacher>().ToTable("Teacher");
            modelBuilder.Entity<Class>().ToTable("Class");
            modelBuilder.Entity<Section>().ToTable("Section");
            modelBuilder.Entity<Configuration>().ToTable("Configuration");
            modelBuilder.Entity<Admission>().ToTable("Admission");
            modelBuilder.Entity<Institute>().ToTable("Institute");
        }

    }
}