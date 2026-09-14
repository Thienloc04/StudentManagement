using StudentManagement.Models.Entities;

namespace StudentManagement.Repositories.Interface
{
    public interface ISubjectRepository
    {
        Task<IEnumerable<Subject>> GetAllSubjectsAsync();

        Task<Subject?> GetSubjectByIdAsync(int id);

        Task<bool> IsSubjectNameExistsAsync(string name, int? excludeId = null);

        Task AddSubjectAsync(Subject subject);

        Task UpdateSubjectAsync(Subject subject);

        Task DeleteSubjectAsync(int id);
        Task<IEnumerable<Subject_Class>> GetSubjectClassesBySubjectIdAsync(int subjectId);
        Task AddSubjectClassAsync(Subject_Class subjectClass);
        Task DeleteSubjectClassAsync(int id);
        Task<IEnumerable<Teacher>> GetAllTeachersAsync();
        Task<bool> IsSubjectClassHasEnrollmentsAsync(int subjectClassId);
        Task<bool> IsSubjectHasClassesAsync(int subjectId);


    }
}
