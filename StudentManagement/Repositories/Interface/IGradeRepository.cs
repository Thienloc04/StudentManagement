using StudentManagement.Models.Entities;

namespace StudentManagement.Repositories.Interface
{
    public interface IGradeRepository
    {
        Task<IEnumerable<Grade>> GetGradesByEnrollmentIdAsync(int enrollmentId);
        Task<Grade?> GetGradeByEnrollmentAndTypeAsync(int enrollmentId, int gradeTypeId);
        Task<IEnumerable<GradeType>> GetAllGradeTypesAsync();
        Task AddGradeAsync(Grade grade);
        Task UpdateGradeAsync(Grade grade);
        Task DeleteGradeAsync(int id);
    }
}
