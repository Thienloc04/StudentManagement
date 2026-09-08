namespace StudentManagement.Models.ViewModels
{
    public class StudentSearchFilterDto
    {
        public string? StudentName { get; set; }
        public int? ClassId { get; set; }
        public string? AcademicRank { get; set; } // "Yếu", "Trung bình", "Khá", "Giỏi"
    }
}
