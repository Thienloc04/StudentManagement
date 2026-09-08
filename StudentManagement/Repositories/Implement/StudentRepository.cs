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

        public async Task<List<StudentSearchResultDto>> SearchStudentsAsync(StudentSearchFilterDto filter)
        {
            // Tạo các tham số cho Stored Procedure (Xử lý Null nếu user không nhập)
            var pStudentName = new SqlParameter("@StudentName", (object?)filter.StudentName ?? DBNull.Value);
            var pClassId = new SqlParameter("@ClassId", (object?)filter.ClassId ?? DBNull.Value);
            var pAcademicRank = new SqlParameter("@AcademicRank", (object?)filter.AcademicRank ?? DBNull.Value);

            // Gọi Stored Procedure sp_SearchStudents
            // SqlQueryRaw<T> cho phép mapping trực tiếp kết quả trả về của 1 Stored Procedure SQL vào List Obj DTO
            var result = await _context.Database
                .SqlQueryRaw<StudentSearchResultDto>(
                "EXEC sp_SearchStudents @StudentName, @ClassId, @AcademicRank",
                pStudentName, pClassId, pAcademicRank)
                .ToListAsync();

            return result;
        }

        public async Task UpdateStudentAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }
    }
}
