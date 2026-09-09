using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models.Entities;
using StudentManagement.Models.ViewModels;
using StudentManagement.Repositories.Interface;

namespace StudentManagement.Repositories.Implement
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;
        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddStudentAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStudentAsync(int id)
        {
            var student = await _context.Students.SingleOrDefaultAsync(x => x.Id == id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            // Dùng .Include() để lấy kèm dữ liệu từ bảng Person (vì Student kế thừa Person)
            // và lấy luôn lịch sử học lớp nào (StudentClasses)
            return await _context.Students
                .Include(s => s.StudentClasses)
                    .ThenInclude(sc => sc.Class)
                .ToListAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _context.Students.Include(s => s.StudentClasses)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<StudentSearchResultDto>> SearchStudentsWithSpAsync(StudentSearchFilterDto filter)
        {
            var pKeyword = new SqlParameter("@Keyword", (object?)filter.Keyword ?? DBNull.Value);
            var pClassId = new SqlParameter("@ClassId", (object?)filter.ClassId ?? DBNull.Value);
            var pRank = new SqlParameter("@AcademicPerformance", (object?)filter.AcademicPerformance ?? DBNull.Value);
            return await _context.Database
                .SqlQueryRaw<StudentSearchResultDto>("EXEC sp_SearchStudents @Keyword, @ClassId, @AcademicPerformance", pKeyword, pClassId, pRank)
                .ToListAsync();
        }

        public async Task UpdateStudentAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }
    }
}
