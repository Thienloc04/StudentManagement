namespace StudentManagement.Models.ViewModels
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        public string StudentNo { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; } //Chuyển đổi từ Sex(bit) sang string
        public string CurrentClassName { get; set; }
        public string StatusName { get; set; }
    }
}
