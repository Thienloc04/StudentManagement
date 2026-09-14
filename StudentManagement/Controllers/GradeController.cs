using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models.ViewModels;
using StudentManagement.Services.Interface;

namespace StudentManagement.Controllers
{
    public class GradeController : Controller
    {
        private readonly IGradeService _gradeService;

        public GradeController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGradeApi(int enrollmentId)
        {
            try
            {
                var result = await _gradeService.GetGradesByEnrollmentAsync(enrollmentId);
                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveGrade([FromBody] GradeSaveDto request)
        {
            try
            {
                await _gradeService.SaveGradeAsync(request);
                return Json(new { success = true, message = "Lưu điểm thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteGradeApi(int id)
        {
            try
            {
                await _gradeService.DeleteGradeAsync(id);
                return Json(new { success = true, message = "Xóa điểm thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        

    }
}
