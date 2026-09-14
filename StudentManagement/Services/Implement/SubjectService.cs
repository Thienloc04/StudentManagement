using StudentManagement.Models.Entities;
using StudentManagement.Models.ViewModels;
using StudentManagement.Repositories.Implement;
using StudentManagement.Repositories.Interface;
using StudentManagement.Services.Interface;

namespace StudentManagement.Services.Implement
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IClassRepository _classRepository;

        public SubjectService(ISubjectRepository subjectRepository, IClassRepository classRepository)
        {
            _subjectRepository = subjectRepository;
            _classRepository = classRepository;
        }

        public async Task<IEnumerable<SubjectViewModel>> GetAllSubjectsAsync()
        {
            var subjects = await _subjectRepository.GetAllSubjectsAsync();

            // Map danh sách Entity -> List ViewModel
            return subjects.Select(s => new SubjectViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Volume = s.Volume,
                Status = s.Status
            });
        }

        public async Task<SubjectViewModel?> GetSubjectByIdAsync(int id)
        {
            var subject = await _subjectRepository.GetSubjectByIdAsync(id);
            if (subject == null) return null;

            return new SubjectViewModel
            {
                Id = subject.Id,
                Name = subject.Name,
                Volume = subject.Volume,
                Status = subject.Status
            };
        }

        public async Task CreateSubjectAsync(SubjectCreateViewModel request)
        {
            // 1. Validate dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new Exception("Tên môn học không được để trống!");
            }

            if (request.Volume <= 0)
            {
                throw new Exception("Số tín chỉ / thời lượng phải lớn hơn 0!");
            }

            // 2. Kiểm tra trùng tên môn học
            if (await _subjectRepository.IsSubjectNameExistsAsync(request.Name))
            {
                throw new Exception($"Tên môn học '{request.Name}' đã tồn tại trong hệ thống!");
            }

            // 3. Tạo Entity mới và lưu vào DB
            var entity = new Subject
            {
                Name = request.Name.Trim(),
                Volume = request.Volume,
                Status = 1, // Mặc định khi tạo mới là 1: Đang giảng dạy
                CreatedDate = DateTime.Now
            };

            await _subjectRepository.AddSubjectAsync(entity);
        }

        public async Task UpdateSubjectAsync(SubjectUpdateViewModel request)
        {
            // 1. Kiểm tra sự tồn tại của môn học
            var existing = await _subjectRepository.GetSubjectByIdAsync(request.Id);
            if (existing == null)
            {
                throw new Exception("Không tìm thấy môn học cần cập nhật!");
            }

            // 2. Validate
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new Exception("Tên môn học không được để trống!");
            }

            if (request.Volume <= 0)
            {
                throw new Exception("Số tín chỉ phải lớn hơn 0!");
            }

            // 3. Kiểm tra trùng tên môn (Bỏ qua tên của chính môn này)
            if (await _subjectRepository.IsSubjectNameExistsAsync(request.Name, request.Id))
            {
                throw new Exception($"Tên môn học '{request.Name}' đã tồn tại!");
            }

            // 4. Cập nhật thông tin
            existing.Name = request.Name.Trim();
            existing.Volume = request.Volume;
            existing.Status = request.Status;

            await _subjectRepository.UpdateSubjectAsync(existing);
        }


        public async Task<IEnumerable<SubjectClassViewModel>> GetSubjectClassesBySubjectIdAsync(int subjectId)
        {
            var list = await _subjectRepository.GetSubjectClassesBySubjectIdAsync(subjectId);

            return list.Select(sc => new SubjectClassViewModel
            {
                SubjectClassId = sc.Id,
                SubjectId = sc.SubjectId,
                SubjectName = sc.Subject?.Name ?? "",
                ClassId = sc.ClassId,
                ClassName = sc.Class?.ClassNo ?? "",
                TeacherId = sc.TeacherId,
                TeacherName = sc.Teacher?.FullName ?? "",
                CreatedDate = sc.CreatedDate
            });
        }

        public async Task CreateSubjectClassAsync(SubjectClassCreateDto request)
        {
            // 1. Validation
            if (request.SubjectId <= 0 || request.ClassId <= 0 || request.TeacherId <= 0)
            {
                throw new Exception("Vui lòng chọn đầy đủ Môn học, Lớp sinh hoạt và Giảng viên!");
            }

            // 2. Kiểm tra xem Lớp HP này đã được mở cho Lớp sinh hoạt & Giảng viên đó chưa
            var existingList = await _subjectRepository.GetSubjectClassesBySubjectIdAsync(request.SubjectId);
            if (existingList.Any(sc => sc.ClassId == request.ClassId && sc.TeacherId == request.TeacherId))
            {
                throw new Exception("Lớp học phần này cho Giảng viên và Lớp sinh hoạt đã chọn đã được mở!");
            }

            // 3. Tạo mới
            var entity = new Subject_Class
            {
                SubjectId = request.SubjectId,
                ClassId = request.ClassId,
                TeacherId = request.TeacherId,
                CreatedDate = DateTime.Now
            };

            await _subjectRepository.AddSubjectClassAsync(entity);
        }

        public async Task DeleteSubjectClassAsync(int id)
        {
            // Kiểm tra nếu đã có học sinh đăng ký -> Báo lỗi
            bool hasEnrollments = await _subjectRepository.IsSubjectClassHasEnrollmentsAsync(id);
            if (hasEnrollments)
            {
                throw new Exception("Không thể xóa lớp học phần này vì đã có học sinh đăng ký học!");
            }

            await _subjectRepository.DeleteSubjectClassAsync(id);
        }

        public async Task DeleteSubjectAsync(int id)
        {
            // Kiểm tra nếu môn học đã được mở Lớp HP -> Không cho xóa môn
            bool hasClasses = await _subjectRepository.IsSubjectHasClassesAsync(id);
            if (hasClasses)
            {
                throw new Exception("Không thể xóa môn học này vì đang có lớp học phần được mở!");
            }

            await _subjectRepository.DeleteSubjectAsync(id);
        }


        public async Task<IEnumerable<ClassViewModel>> GetClassesDropdownAsync()
        {
            var classes = await _classRepository.GetAllClassesAsync();
            return classes.Select(c => new ClassViewModel
            {
                Id = c.Id,
                ClassNo = c.ClassNo
            });
        }

        public async Task<IEnumerable<object>> GetTeachersDropdownAsync()
        {
            var teachers = await _subjectRepository.GetAllTeachersAsync();
            return teachers.Select(t => new
            {
                id = t.Id,
                fullName = t.TeacherNo + " - " + t.FullName
            });
        }

    }
}
