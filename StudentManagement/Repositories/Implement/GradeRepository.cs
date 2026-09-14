using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models.Entities;
using StudentManagement.Repositories.Interface;

namespace StudentManagement.Repositories.Implement
{
    public class GradeRepository : IGradeRepository
    {
        private readonly AppDbContext _context;
        public GradeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddGradeAsync(Grade grade)
        {
            await _context.Grades.AddAsync(grade);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGradeAsync(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if(grade != null)
            {
                _context.Grades.Remove(grade);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<GradeType>> GetAllGradeTypesAsync()
        {
            return await _context.GradeTypes.ToListAsync();
        }

        public async Task<Grade?> GetGradeByEnrollmentAndTypeAsync(int enrollmentId, int gradeTypeId)
        {
            return await _context.Grades.FirstOrDefaultAsync(g => g.EnrollmentId == enrollmentId && g.GradeTypeId == gradeTypeId);
        }

        public async Task<IEnumerable<Grade>> GetGradesByEnrollmentIdAsync(int enrollmentId)
        {
            return await _context.Grades
                .Include(g => g.GradeType)
                .Where(g => g.EnrollmentId == enrollmentId)
                .ToListAsync();
        }

        public async Task UpdateGradeAsync(Grade grade)
        {
            _context.Grades.Update(grade);
            await _context.SaveChangesAsync();
        }
    }
}
