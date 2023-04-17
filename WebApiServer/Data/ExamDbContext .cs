using Microsoft.EntityFrameworkCore;
using WebApiServer.Models;

namespace WebApiServer.Data
{
    public class ExamDbContext : DbContext
    {
        public ExamDbContext(DbContextOptions<ExamDbContext> options)
            : base(options)
        {

        }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Exam entity
            modelBuilder.Entity<Exam>()
                .HasKey(e => e.Id); // Set the primary key for the Exam entity

            // Configure the Question entity
            modelBuilder.Entity<Question>()
                .HasKey(q => q.Id); // Set the primary key for the Question entity

            // Configure the Option entity
            modelBuilder.Entity<Option>()
                .HasKey(o => o.Id); // Set the primary key for the Option entity

            // Configure the ExamResult entity
            modelBuilder.Entity<ExamResult>()
                .HasKey(er => er.Id); // Set the primary key for the ExamResult entity

            // Configure the Error entity
            modelBuilder.Entity<Error>()
                .HasKey(er => er.Id); // Set the primary key for the Error entity
        }
    }
}
