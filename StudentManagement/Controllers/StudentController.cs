using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models.ViewModels;
using StudentManagement.Services.Exporters;
using StudentManagement.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IClassService _classService;
        private readonly IEnumerable<IStudentExporter> _exporters;

        public StudentController(
            IStudentService studentService, 
            IClassService classService,
            IEnumerable<IStudentExporter> exporters)
        {
            _studentService = studentService;
            _classService = classService;
            _exporters = exporters;
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

        // API search student theo keyword/học lực/lớp
        [HttpGet]
        public async Task<IActionResult> SearchStudentsApi([FromQuery] StudentSearchFilterDto filter)
        {
            try
            {
                var result = await _studentService.SearchStudentsAsync(filter);
                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // API lấy danh sách lớp học để đổ vào dropdown tìm kiếm
        [HttpGet]
        public async Task<IActionResult> GetClassesApi()
        {
            try
            {
                var result = await _classService.GetClassesListAsync();
                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message });
            }
        }

        // Action Xuất Báo Cáo File (PDF / EXCEL) áp dụng Strategy Pattern (OCP)
        [HttpGet]
        public async Task<IActionResult> ExportStudents([FromQuery] string format, [FromQuery] StudentSearchFilterDto filter)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = "PDF";
            }

            // 1. Lấy dữ liệu danh sách học sinh theo bộ lọc
            var students = await _studentService.SearchStudentsAsync(filter);

            // 2. TÌM EXPORTER PHÙ HỢP: Tự động tìm Strategy matching với 'format' trong IEnumerable<IStudentExporter>
            var exporter = _exporters.FirstOrDefault(e => e.ExportFormat.Equals(format, StringComparison.OrdinalIgnoreCase));

            if (exporter == null)
            {
                return BadRequest($"Định dạng xuất file '{format}' không được hỗ trợ.");
            }

            // 3. Chuẩn bị Tiêu đề báo cáo
            string title = "BÁO CÁO THỐNG KÊ DANH SÁCH HỌC SINH";
            if (filter.ClassId.HasValue && filter.ClassId.Value > 0)
            {
                var classList = await _classService.GetClassesListAsync();
                var currentClass = classList.FirstOrDefault(c => c.Id == filter.ClassId.Value);
                if (currentClass != null)
                {
                    title = $"BÁO CÁO THỐNG KÊ HỌC SINH LỚP {currentClass.ClassNo}";
                }
            }

            // 4. Tiến hành xuất file mảng byte
            byte[] fileBytes = exporter.Export(students, title);
            string fileName = $"DanhSachHocSinh_{DateTime.Now:yyyyMMdd_HHmmss}.{exporter.FileExtension}";

            // Trả file về cho trình duyệt tự động tải xuống
            return File(fileBytes, exporter.ContentType, fileName);
        }
    }
}

