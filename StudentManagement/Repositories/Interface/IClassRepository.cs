using StudentManagement.Models.Entities;

namespace StudentManagement.Repositories.Interface
{
    public interface IClassRepository
    {
        Task<IEnumerable<Class>> GetAllClassesAsync();
    }
}
