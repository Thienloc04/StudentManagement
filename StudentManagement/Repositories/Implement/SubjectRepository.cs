using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models.Entities;
using StudentManagement.Repositories.Interface;

namespace StudentManagement.Repositories.Implement
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly AppDbContext _context;

        public SubjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Subject>> GetAllSubjectsAsync()
        {
            return await _context.Subjects.ToListAsync();
        }

        public async Task<Subject?> GetSubjectByIdAsync(int id)
        {
            return await _context.Subjects.FindAsync(id);
        }

        public async Task<bool> IsSubjectNameExistsAsync(string name, int? excludeId = null)
        {
            if (excludeId.HasValue)
            {
                // Dùng khi Cập nhật: Kiểm tra trùng tên nhưng bỏ qua chính môn học đang chỉnh sửa (excludeId)
                return await _context.Subjects.AnyAsync(s => s.Name.ToLower() == name.ToLower() && s.Id != excludeId.Value);
            }
            // Dùng khi Thêm mới: Kiểm tra trùng tên
            return await _context.Subjects.AnyAsync(s => s.Name.ToLower() == name.ToLower());
        }

        public async Task AddSubjectAsync(Subject subject)
        {
            await _context.Subjects.AddAsync(subject);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubjectAsync(Subject subject)
        {
            _context.Subjects.Update(subject);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubjectAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Subject_Class>> GetSubjectClassesBySubjectIdAsync(int subjectId)
        {
            return await _context.SubjectClasses
                .Include(sc => sc.Subject)
                .Include(sc => sc.Class)
                .Include(sc => sc.Teacher)
                .Where(sc => sc.SubjectId == subjectId)
                .ToListAsync();
        }

        public async Task AddSubjectClassAsync(Subject_Class subjectClass)
        {
            await _context.SubjectClasses.AddAsync(subjectClass);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubjectClassAsync(int id)
        {
            var item = await _context.SubjectClasses.FindAsync(id);
            if (item != null)
            {
                _context.SubjectClasses.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Teacher>> GetAllTeachersAsync()
        {
            return await _context.Teachers.Where(t => t.Status == 1).ToListAsync();
        }
        public async Task<bool> IsSubjectClassHasEnrollmentsAsync(int subjectClassId)
        {
            return await _context.Enrollments.AnyAsync(e => e.SubjectClassId == subjectClassId);
        }

        public async Task<bool> IsSubjectHasClassesAsync(int subjectId)
        {
            return await _context.SubjectClasses.AnyAsync(sc => sc.SubjectId == subjectId);
        }

    }
}
