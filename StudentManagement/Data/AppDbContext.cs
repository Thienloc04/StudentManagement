using Microsoft.EntityFrameworkCore;
using StudentManagement.Models.Entities;

namespace StudentManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. TẮT CASCADE DELETE CHO ENROLLMENT VÀ STUDENT_CLASS
            // Chặn xóa dây chuyền từ Student -> Enrollment
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict: Cấm xóa Student nếu còn Enrollment

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.SubjectClass)
                .WithMany(sc => sc.Enrollments)
                .HasForeignKey(e => e.SubjectClassId)
                .OnDelete(DeleteBehavior.Restrict);

            // Chặn xóa dây chuyền từ Student -> Student_Class (để an toàn tuyệt đối)
            modelBuilder.Entity<Student_Class>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentClasses)
                .HasForeignKey(sc => sc.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. KHẮC PHỤC CẢNH BÁO DECIMAL
            modelBuilder.Entity<Grade>()
                .Property(g => g.Score)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<GradeType>()
                .Property(gt => gt.Weight)
                .HasColumnType("decimal(18,2)");

        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Student_Class> StudentClasses { get; set; }
        public DbSet<Subject_Class> SubjectClasses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<GradeType> GradeTypes { get; set; }

    }
}
