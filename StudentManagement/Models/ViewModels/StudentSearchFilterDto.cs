namespace StudentManagement.Models.ViewModels
{
    public class StudentSearchFilterDto
    {
        public string? Keyword { get; set; }
        public int? ClassId { get; set; }
        public string? AcademicPerformance { get; set; } // "Yếu", "Trung bình", "Khá", "Giỏi"
    }
}
