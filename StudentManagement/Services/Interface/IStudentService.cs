using StudentManagement.Models.ViewModels;

namespace StudentManagement.Services.Interface
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentViewModel>> GetStudentListAsync();
    }
}
