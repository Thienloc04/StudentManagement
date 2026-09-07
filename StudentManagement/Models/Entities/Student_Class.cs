using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models.Entities
{
    [Table("Student_Classes")]
    public class Student_Class
    {
        [Key]
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public int AcademicYear { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; } // Có thể null vì chưa kết thúc

        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }

        [ForeignKey(nameof(ClassId))]
        public virtual Class Class { get; set; }
    }
}
