namespace StudentManagement.Models.ViewModels
{
    public class SubjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Volume { get; set; }
        public int Status { get; set; } // 1: Đang giảng dạy; 2: Tạm ngưng
    }
}
