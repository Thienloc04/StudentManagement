namespace StudentManagement.Models.ViewModels
{
    public class StudentCreateViewModel
    {
        public string StudentNo { get; set; }
        public string FullName { get; set; }
        public bool Sex { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
