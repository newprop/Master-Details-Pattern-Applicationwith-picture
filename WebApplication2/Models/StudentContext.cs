using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models
{
    public class StudentContext:DbContext
    {
        public DbSet<Students> Students { get; set; }
        public DbSet<Subject> Subjectlists { get; set; }
        public DbSet<StudentExamMarks> StudentMarks { get; set; }

        public StudentContext(DbContextOptions opt) : base(opt)
        {



        }
    }
}
