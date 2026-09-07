using StudentManagement.Models.ViewModels;
using StudentManagement.Repositories.Interface;
using StudentManagement.Services.Interface;

namespace StudentManagement.Services.Implement
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<StudentViewModel>> GetStudentListAsync()
        {
            var rawStudents = await _studentRepository.GetAllStudentsAsync();

            var result = rawStudents.Select(s =>
            {
                var currentClass = s.StudentClasses.FirstOrDefault(sc => sc.EndDate == null)?.Class.ClassNo ?? "Chưa xếp lớp";

                return new StudentViewModel
                {
                    Id = s.Id,
                    StudentNo = s.StudentNo,
                    FullName = s.FullName,
                    Gender = s.Sex ? "Nam" : "Nữ",
                    CurrentClassName = currentClass,
                    StatusName = s.Status == 1 ? "Đang học" : "Đã nghỉ"

                };
            });

            return result;
        }
    }
}
