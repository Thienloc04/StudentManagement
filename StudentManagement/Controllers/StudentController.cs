using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models.ViewModels;
using StudentManagement.Services.Interface;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // 1. Hàm này trả về giao diện HTML (Khi người dùng gõ URL trên trình duyệt)
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // 2. Hàm này làm Endpoint cho AJAX gọi (chỉ trả về Data)
        public async Task<IActionResult> GetStudentDataApi()
        {
            // Gọi xuống tầng Service để lấy danh sách ViewModel
            var data = await _studentService.GetStudentListAsync();

            // ASP.NET tự động dịch List<StudentViewModel> thành chuỗi JSON
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudentApi([FromBody] StudentCreateViewModel request)
        {
            try
            {
                await _studentService.CreateStudentAsync(request);

                return Json(new { success = true, message = "Thêm học sinh thành công!"});
            }
            catch (Exception ex) 
            {
                // Nếu có lỗi (VD: trùng mã số), trả về false
                return Json(new { success = false, message = ex.Message });
            }
        }

        //API lấy data đổ lên form
        [HttpGet]
        public async Task<IActionResult> GetStudentByIdApi(int id)
        {
            try
            {
                var data = await _studentService.GetStudentForEditAsync(id);
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message });
            }
        }

        // API cập nhật (dùng httpPut)
        [HttpPut]
        public async Task<IActionResult> UpdateStudentApi([FromBody]StudentUpdateViewModel request)
        {
            try
            {
                await _studentService.UpdateStudentAsync(request);
                return Json(new { success = true, message = "Cập nhật thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        // API Xóa (Dùng Http Delete)
        [HttpDelete]
        public async Task<IActionResult> DeleteStudentApi(int id)
        {
            try
            {
                await _studentService.DeleteStudentAsync(id);
                return Json(new { success = true, message = "Xóa thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
