namespace StudentManagement.Models.ViewModels
{
    public class GradeItemDto
    {
        public int Id { get; set; } 
        public int EnrollmentId { get; set; }
        public int GradeTypeId { get; set; }
        public string GradeTypeName { get; set; }
        public decimal Weight { get; set; }
        public decimal Score { get; set; }
    }
}
