using StudentManagement.Models.Entities;
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

        public async Task CreateStudentAsync(StudentCreateViewModel model)
        {
            var newStudent = new Student
            {
                StudentNo = model.StudentNo,
                FullName = model.FullName,
                Sex = model.Sex,
                Phone = model.Phone,
                Email = model.Email,
                Address = model.Address,
                Status = 1,
                CreatedDate = DateTime.Now,
            };

            // Gọi Repository để lưu
            await _studentRepository.AddStudentAsync(newStudent);
        }

        public async Task DeleteStudentAsync(int id)
        {
            await _studentRepository.DeleteStudentAsync(id);
        }

        public async Task<StudentUpdateViewModel> GetStudentForEditAsync(int id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null)
                throw new Exception("Không tìm thấy học sinh");

            return new StudentUpdateViewModel
            {
                Id = id,
                StudentNo = student.StudentNo,
                FullName = student.FullName,
                Sex = student.Sex,
                Phone = student.Phone,
                Email = student.Email,
                Address = student.Address,
                DateOfBirth = student.DateOfBirth,
            };
        }

        public async Task<IEnumerable<StudentViewModel>> GetStudentListAsync()
        {
            // 1. Lấy dữ liệu thô từ repository
            var rawStudents = await _studentRepository.GetAllStudentsAsync();
            
            // 2. Chế biến và map dữ liệu
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

        public async Task UpdateStudentAsync(StudentUpdateViewModel model)
        {
            var student = await _studentRepository.GetStudentByIdAsync(model.Id);
            if (student == null) throw new Exception("Không tìm thấy học sinh");

            // Update fields
            student.StudentNo = model.StudentNo;
            student.FullName = model.FullName;
            student.Sex = model.Sex;
            student.Phone = model.Phone;
            student.Email = model.Email;
            student.Address = model.Address;
            student.DateOfBirth = model.DateOfBirth;

            await _studentRepository.UpdateStudentAsync(student);
        }
    }
}
