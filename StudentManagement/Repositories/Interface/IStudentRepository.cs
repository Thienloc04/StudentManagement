using StudentManagement.Models.Entities;
using StudentManagement.Models.ViewModels;

namespace StudentManagement.Repositories.Interface
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);
        Task AddStudentAsync(Student student);
        Task UpdateStudentAsync(Student student);
        Task DeleteStudentAsync(int id);
        Task<List<StudentSearchResultDto>> SearchStudentsAsync(StudentSearchFilterDto filter);
    }
}
