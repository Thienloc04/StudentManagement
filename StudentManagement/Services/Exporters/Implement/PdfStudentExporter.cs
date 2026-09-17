using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using StudentManagement.Models.ViewModels;

namespace StudentManagement.Services.Exporters.Implement
{
    public class PdfStudentExporter : IStudentExporter
    {
        public string ExportFormat => "PDF";
        public string ContentType => "application/pdf";
        public string FileExtension => "pdf";

        public byte[] Export(IEnumerable<StudentSearchResultDto> students, string title = "DANH SÁCH HỌC SINH")
        {
            QuestPDF.Settings.License = LicenseType.Community;
            QuestPDF.Settings.CheckIfAllTextGlyphsAreAvailable = false;

            var studentList = students?.ToList() ?? new List<StudentSearchResultDto>();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1.5f, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    // Cấu hình Font chữ Arial (có sẵn trên hệ thống) để hiển thị đầy đủ tiếng Việt có dấu
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(11));

                    page.Header()
                        .PaddingBottom(10)
                        .AlignCenter()
                        .Text(title)
                        .SemiBold().FontSize(18).FontColor(Colors.Blue.Darken3);

                    page.Content()
                        .PaddingVertical(10)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(35);  // STT
                                columns.ConstantColumn(75);  // Mã HS
                                columns.RelativeColumn(3);   // Họ tên
                                columns.ConstantColumn(55);  // Giới tính
                                columns.ConstantColumn(85);  // Lớp
                                columns.ConstantColumn(60);  // Điểm TB
                                columns.ConstantColumn(75);  // Xếp loại
                            });

                            // Table Header
                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderStyle).Text("STT").Bold();
                                header.Cell().Element(HeaderStyle).Text("Mã HS").Bold();
                                header.Cell().Element(HeaderStyle).Text("Họ và tên").Bold();
                                header.Cell().Element(HeaderStyle).Text("Giới tính").Bold();
                                header.Cell().Element(HeaderStyle).Text("Lớp").Bold();
                                header.Cell().Element(HeaderStyle).Text("Điểm TB").Bold();
                                header.Cell().Element(HeaderStyle).Text("Xếp loại").Bold();

                                static IContainer HeaderStyle(IContainer c) =>
                                    c.Background(Colors.Grey.Lighten2).Padding(6);
                            });

                            // Table Rows
                            int stt = 1;
                            foreach (var s in studentList)
                            {
                                table.Cell().Element(RowStyle).Text(stt++.ToString());
                                table.Cell().Element(RowStyle).Text(s.StudentNo ?? "");
                                table.Cell().Element(RowStyle).Text(s.FullName ?? "");
                                table.Cell().Element(RowStyle).Text(s.Sex ? "Nam" : "Nữ");
                                table.Cell().Element(RowStyle).Text(s.ClassName ?? "Chưa xếp lớp");
                                table.Cell().Element(RowStyle).Text(s.AverageScore.ToString("F2"));
                                table.Cell().Element(RowStyle).Text(s.AcademicPerformance ?? "");

                                static IContainer RowStyle(IContainer c) =>
                                    c.BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(6);
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Trang ");
                            x.CurrentPageNumber();
                            x.Span(" / ");
                            x.TotalPages();
                        });
                });
            });

            return document.GeneratePdf();
        }
    }
}
