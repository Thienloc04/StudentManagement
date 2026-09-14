namespace StudentManagement.Models.ViewModels
{
    public class GradeSaveDto
    {
        public int? Id { get; set; }
        public int EnrollmentId { get; set; }
        public int GradeTypeId { get; set; }
        public decimal Score { get; set; }
    }
}
