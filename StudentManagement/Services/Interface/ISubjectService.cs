using StudentManagement.Models.ViewModels;

namespace StudentManagement.Services.Interface
{
    public interface ISubjectService
    {
        Task<IEnumerable<SubjectViewModel>> GetAllSubjectsAsync();  
        Task<SubjectViewModel?> GetSubjectByIdAsync(int id);
        Task CreateSubjectAsync(SubjectCreateViewModel request);
        Task UpdateSubjectAsync(SubjectUpdateViewModel request);
        Task DeleteSubjectAsync(int id);
        Task<IEnumerable<SubjectClassViewModel>> GetSubjectClassesBySubjectIdAsync(int subjectId);
        Task CreateSubjectClassAsync(SubjectClassCreateDto request);
        Task DeleteSubjectClassAsync(int id);
        Task<IEnumerable<ClassViewModel>> GetClassesDropdownAsync();
        Task<IEnumerable<object>> GetTeachersDropdownAsync();

    }
}
