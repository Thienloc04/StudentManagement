using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models.Entities;
using StudentManagement.Models.ViewModels;
using StudentManagement.Repositories.Interface;

namespace StudentManagement.Repositories.Implement
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly AppDbContext _context;
        public EnrollmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddEnrollmentAsync(Enrollment enrollment)
        {
            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteEnrollmentAsync(int enrollmentId)
        {
            var item = await _context.Enrollments.FindAsync(enrollmentId);

            if(item != null)
            {
                _context.Enrollments.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetEnrollmentsByStudentIdAsync(int studentId)
        {
            return await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.SubjectClass)
                    .ThenInclude(sc => sc.Subject)
                .Include(e => e.SubjectClass)
                    .ThenInclude(sc => sc.Teacher)
                .AsSplitQuery()

                .Where(e => e.StudentId == studentId)
                .Select(e => new EnrollmentViewModel
                {
                    EnrollmentId = e.Id,
                    StudentId = e.StudentId,
                    SubjectName = e.SubjectClass.Subject.Name,
                    Volume = e.SubjectClass.Subject.Volume,
                    TeacherName = e.SubjectClass.Teacher.FullName,
                    ClassName = e.SubjectClass.Class.ClassNo,
                    RegisterDate = e.RegisterDate
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetSubjectClassesDropdownAsync()
        {
            return await _context.SubjectClasses
                .Include(sc => sc.Subject)
                .Include(sc => sc.Teacher)
                .Include(sc => sc.Class)
                .Select(sc => new
                {
                    SubjectClassId = sc.Id,
                    DisplayText = $"{sc.Subject.Name} - GV: {sc.Teacher.FullName} ({sc.Class.ClassNo})"
                })
                .ToListAsync();
        }

        public async Task<bool> IsAlreadyEnrolledAsync(int studentId, int subjectClassId)
        {
            return await _context.Enrollments.AnyAsync(e => e.StudentId == studentId && e.SubjectClassId == subjectClassId);
        }
    }
}
