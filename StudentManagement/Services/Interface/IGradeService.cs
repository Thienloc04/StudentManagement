using StudentManagement.Models.ViewModels;

namespace StudentManagement.Services.Interface
{
    public interface IGradeService
    {
        Task<IEnumerable<GradeItemDto>> GetGradesByEnrollmentAsync(int enrollmentId);
        Task SaveGradeAsync(GradeSaveDto request);
        Task DeleteGradeAsync(int id);
    }
}
