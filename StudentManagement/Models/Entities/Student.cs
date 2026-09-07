using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models.Entities
{
    [Table("Students")]
    public class Student : Person
    {
        [Required]
        [StringLength(20)]
        public string StudentNo { get; set; }
        public int Status { get; set; } // 1: Đang học 0: Đã nghỉ

        public virtual ICollection<Student_Class> StudentClasses { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; } 
    }
}
