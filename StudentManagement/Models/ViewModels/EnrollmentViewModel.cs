namespace StudentManagement.Models.ViewModels
{
    public class EnrollmentViewModel
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public string SubjectName { get; set; }
        public int Volume { get; set; }
        public String TeacherName { get; set; }
        public string ClassName { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
