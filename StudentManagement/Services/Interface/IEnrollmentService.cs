using StudentManagement.Models.Entities;
using StudentManagement.Models.ViewModels;

namespace StudentManagement.Services.Interface
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentViewModel>> GetStudentEnrollmentsAsync(int studentId);
        Task<IEnumerable<object>> GetSubjectClassesDropdownAsync();
        Task EnrollSubjectAsync(EnrollmentCreateDto dto);
        Task CancelEnrollmentAsync(int enrollmentId);
    }
}
