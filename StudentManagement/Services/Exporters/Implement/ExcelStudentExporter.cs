using ClosedXML.Excel;
using StudentManagement.Models.ViewModels;

namespace StudentManagement.Services.Exporters.Implement
{
    public class ExcelStudentExporter : IStudentExporter
    {
        public string ExportFormat => "EXCEL";
        public string ContentType => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public string FileExtension => "xlsx";

        public byte[] Export(IEnumerable<StudentSearchResultDto> students, string title = "DANH SÁCH HỌC SINH")
        {
            var studentList = students?.ToList() ?? new List<StudentSearchResultDto>();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Danh Sách Học Sinh");

                // Title Row
                worksheet.Cell(1, 1).Value = title;
                worksheet.Range(1, 1, 1, 7).Merge();
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Font.FontSize = 16;
                worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Header Row
                string[] headers = { "STT", "Mã HS", "Họ và tên", "Giới tính", "Lớp hiện tại", "Điểm TB", "Xếp loại" };
                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = worksheet.Cell(3, i + 1);
                    cell.Value = headers[i];
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#0D6EFD");
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                // Data Rows
                int row = 4;
                int stt = 1;
                foreach (var s in studentList)
                {
                    worksheet.Cell(row, 1).Value = stt++;
                    worksheet.Cell(row, 2).Value = s.StudentNo ?? "";
                    worksheet.Cell(row, 3).Value = s.FullName ?? "";
                    worksheet.Cell(row, 4).Value = s.Sex ? "Nam" : "Nữ";
                    worksheet.Cell(row, 5).Value = s.ClassName ?? "Chưa xếp lớp";
                    worksheet.Cell(row, 6).Value = s.AverageScore;
                    worksheet.Cell(row, 6).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(row, 7).Value = s.AcademicPerformance ?? "";
                    row++;
                }

                // Auto-fit Columns
                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
