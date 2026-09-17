using StudentManagement.Models.ViewModels;

namespace StudentManagement.Services.Exporters
{
    public interface IStudentExporter
    {
        /// <summary>
        /// Định danh loại file (VD: "PDF", "EXCEL")
        /// </summary>
        string ExportFormat { get; }

        /// <summary>
        /// Loại nội dung trả về cho trình duyệt (VD: "application/pdf")
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Đuôi file mở rộng (VD: "pdf", "xlsx")
        /// </summary>
        string FileExtension { get; }

        /// <summary>
        /// Hàm xử lý chính: Nhận danh sách Data và trả về mảng byte (file)
        /// </summary>
        byte[] Export(IEnumerable<StudentSearchResultDto> students, string title = "DANH SÁCH HỌC SINH");
    }
}
