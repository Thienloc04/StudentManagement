namespace StudentManagement.Models.ViewModels
{
    public class SubjectUpdateViewModel : SubjectCreateViewModel
    {
        public int Id { get; set; }
        public int Status { get; set; }
    }
}
