using Microsoft.EntityFrameworkCore;

namespace TrainingManagement.CourseApi.Models
{
    public class CourseDbContext:DbContext
    {
        public CourseDbContext(DbContextOptions<CourseDbContext> options) : base(options)
        {
        }
        public DbSet<Data.Course> Courses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Data.Course>().HasData(
                new Data.Course { CourseId = 1, CourseName = "C# Basics", CourseDescription = "Learn the basics of C#", Duration = 10 },
                new Data.Course { CourseId = 2, CourseName = "ASP.NET Core", CourseDescription = "Learn how to build web applications with ASP.NET Core", Duration = 15 },
                new Data.Course { CourseId = 3, CourseName = "Entity Framework Core", CourseDescription = "Learn how to use Entity Framework Core for data access", Duration = 12 }
            );
        }
    }
}
