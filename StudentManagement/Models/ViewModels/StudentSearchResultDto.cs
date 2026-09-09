namespace StudentManagement.Models.ViewModels
{
    public class StudentSearchResultDto
    {
        public int StudentId { get; set; }
        public string StudentNo { get; set; }
        public string FullName { get; set; }
        public bool Sex { get; set; }
        public string ClassName { get; set; }
        public decimal AverageScore { get; set; }
        public string AcademicPerformance { get; set; }
    }
}
