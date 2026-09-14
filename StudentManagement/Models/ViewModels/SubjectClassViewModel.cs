namespace StudentManagement.Models.ViewModels
{
    public class SubjectClassViewModel
    {
        public int SubjectClassId { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}
