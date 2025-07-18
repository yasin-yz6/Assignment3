using Microsoft.EntityFrameworkCore;
using University.Data.Contexts.Mappings;
using University.Data.Entities;

namespace University.Data.Contexts
{
    public class UniversityDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StudentMapping());
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Server=YASIN_PAVILION\\SQLEXPRESS;Database=UniveristyAPI;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}
