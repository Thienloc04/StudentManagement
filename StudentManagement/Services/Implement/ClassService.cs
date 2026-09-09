using StudentManagement.Models.ViewModels;
using StudentManagement.Repositories.Interface;
using StudentManagement.Services.Interface;

namespace StudentManagement.Services.Implement
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;
        public ClassService(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public async Task<IEnumerable<ClassViewModel>> GetClassesListAsync()
        {
            var rawData = await _classRepository.GetAllClassesAsync();

            var classList = rawData.Select(c =>
            {
                return new ClassViewModel
                {
                    Id = c.Id,
                    ClassNo = c.ClassNo,
                };
            });

            return classList;
        }
    }
}
