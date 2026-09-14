using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models.ViewModels
{
    public class SubjectCreateViewModel
    {
        [Required(ErrorMessage = "Tên môn học không được để trống")]
        [StringLength(100, ErrorMessage = "Tên môn học không được vượt quá 100 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số tín chỉ")]
        [Range(1, 10, ErrorMessage = "Số tín chỉ phải từ 1 đến 10")]
        public int Volume { get; set; }
    }
}
