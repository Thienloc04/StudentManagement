using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models.ViewModels;
using StudentManagement.Services.Interface;

namespace StudentManagement.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;
        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }
        public IActionResult Index()
        {
            return View();
        }

        // API lấy danh sách Lớp học phần đang mở
        [HttpGet]
        public async Task<IActionResult> GetSubjectClassesDropdownApi()
        {
            var result = await _enrollmentService.GetSubjectClassesDropdownAsync();
            return Json(new { success = true, data = result });
        }

        // API lấy các môn học mà 1 học sinh cụ thể đã đăng ký
        [HttpGet]
        public async Task<IActionResult> GetStudentEnrollmentsApi(int studentId)
        {
            var result = await _enrollmentService.GetStudentEnrollmentsAsync(studentId);
            return Json(new { success = true, data = result });
        }

        // API thực hiện Đăng ký môn học
        [HttpPost]
        public async Task<IActionResult> RegisterSubjectApi([FromBody] EnrollmentCreateDto request)
        {
            try
            {
                await _enrollmentService.EnrollSubjectAsync(request);
                return Json(new { success = true, message = "Đăng ky môn học thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // API Hủy đăng ký môn học
        [HttpDelete]
        public async Task<IActionResult> CancelEnrollmentApi(int id)
        {
            try
            {
                await _enrollmentService.CancelEnrollmentAsync(id);
                return Json(new { success = true, message = "Hủy đăng ký môn học thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
