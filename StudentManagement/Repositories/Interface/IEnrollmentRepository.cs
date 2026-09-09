using StudentManagement.Models.Entities;
using StudentManagement.Models.ViewModels;

namespace StudentManagement.Repositories.Interface
{
    public interface IEnrollmentRepository
    {
        Task<IEnumerable<EnrollmentViewModel>> GetEnrollmentsByStudentIdAsync (int studentId);
        Task<bool> IsAlreadyEnrolledAsync (int studentId, int subjectClassId);
        Task AddEnrollmentAsync(Enrollment enrollment);
        Task DeleteEnrollmentAsync (int enrollmentId);
        Task<IEnumerable<object>> GetSubjectClassesDropdownAsync();
    }
}
