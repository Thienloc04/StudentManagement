using StudentManagement.Models.ViewModels;

namespace StudentManagement.Services.Interface
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentViewModel>> GetStudentListAsync();
        Task CreateStudentAsync(StudentCreateViewModel model);
        Task<StudentUpdateViewModel> GetStudentForEditAsync(int id);
        Task UpdateStudentAsync(StudentUpdateViewModel model);
        Task DeleteStudentAsync(int id);
        Task<IEnumerable<StudentSearchResultDto>> SearchStudentsAsync(StudentSearchFilterDto filter);
    }
}
