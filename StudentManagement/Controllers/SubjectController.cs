using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models.ViewModels;
using StudentManagement.Services.Interface;

namespace StudentManagement.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // API Lấy danh sách tất cả môn học
        [HttpGet]
        public async Task<IActionResult> GetSubjectDataApi()
        {
            try
            {
                var data = await _subjectService.GetAllSubjectsAsync();
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // API Lấy chi tiết 1 môn học để đổ lên Form sửa
        [HttpGet]
        public async Task<IActionResult> GetSubjectByIdApi(int id)
        {
            try
            {
                var data = await _subjectService.GetSubjectByIdAsync(id);
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // API Thêm mới môn học
        [HttpPost]
        public async Task<IActionResult> AddSubjectApi([FromBody] SubjectCreateViewModel request)
        {
            try
            {
                await _subjectService.CreateSubjectAsync(request);
                return Json(new { success = true, message = "Thêm môn học thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // API Cập nhật môn học
        [HttpPut]
        public async Task<IActionResult> UpdateSubjectApi([FromBody] SubjectUpdateViewModel request)
        {
            try
            {
                await _subjectService.UpdateSubjectAsync(request);
                return Json(new { success = true, message = "Cập nhật môn học thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // API Xóa môn học
        [HttpDelete]
        public async Task<IActionResult> DeleteSubjectApi(int id)
        {
            try
            {
                await _subjectService.DeleteSubjectAsync(id);
                return Json(new { success = true, message = "Xóa môn học thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // API Lấy danh sách các Lớp Học Phần đã mở của 1 môn học
        [HttpGet]
        public async Task<IActionResult> GetSubjectClassesApi(int subjectId)
        {
            try
            {
                var data = await _subjectService.GetSubjectClassesBySubjectIdAsync(subjectId);
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // API Mở Lớp Học Phần mới
        [HttpPost]
        public async Task<IActionResult> AddSubjectClassApi([FromBody] SubjectClassCreateDto request)
        {
            try
            {
                await _subjectService.CreateSubjectClassAsync(request);
                return Json(new { success = true, message = "Mở lớp học phần thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // API Xóa Lớp Học Phần
        [HttpDelete]
        public async Task<IActionResult> DeleteSubjectClassApi(int id)
        {
            try
            {
                await _subjectService.DeleteSubjectClassAsync(id);
                return Json(new { success = true, message = "Xóa lớp học phần thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // API Lấy danh sách Dropdown (Lớp sinh hoạt & Giảng viên) để chọn khi mở lớp
        [HttpGet]
        public async Task<IActionResult> GetDropdownDataApi()
        {
            try
            {
                var classes = await _subjectService.GetClassesDropdownAsync();
                var teachers = await _subjectService.GetTeachersDropdownAsync();
                return Json(new { success = true, classes = classes, teachers = teachers });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
