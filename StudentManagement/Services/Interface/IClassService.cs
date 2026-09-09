using StudentManagement.Models.ViewModels;

namespace StudentManagement.Services.Interface
{
    public interface IClassService
    {
        Task<IEnumerable<ClassViewModel>> GetClassesListAsync();
    }
}
