using StudentManagement.Models.Entities;
using StudentManagement.Models.ViewModels;
using StudentManagement.Repositories.Interface;
using StudentManagement.Services.Interface;

namespace StudentManagement.Services.Implement
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        public EnrollmentService(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task CancelEnrollmentAsync(int enrollmentId)
        {
            await _enrollmentRepository.DeleteEnrollmentAsync(enrollmentId);
        }

        public async Task EnrollSubjectAsync(EnrollmentCreateDto dto)
        {
            // Kiểm tra trùng lặp: Sinh viên đã đăng ký lớp học phần này chưa?
            var isEnrolled = await _enrollmentRepository.IsAlreadyEnrolledAsync(dto.StudentId, dto.SubjectClassId);

            if (isEnrolled)
            {
                throw new Exception("Học sinh này đã đăng ký học phần này rồi!");
            }

            var enrollment = new Enrollment
            {
                StudentId = dto.StudentId,
                SubjectClassId = dto.SubjectClassId,
                RegisterDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(4)
            };

            await _enrollmentRepository.AddEnrollmentAsync(enrollment);
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetStudentEnrollmentsAsync(int studentId)
        {
            return await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
        }

        public async Task<IEnumerable<object>> GetSubjectClassesDropdownAsync()
        {
            return await _enrollmentRepository.GetSubjectClassesDropdownAsync();
        }
    }
}
