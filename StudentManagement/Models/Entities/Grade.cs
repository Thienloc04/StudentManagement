using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models.Entities
{
    [Table("Grades")]
    public class Grade
    {
        [Key]
        public int Id { get; set; }
        public int EnrollmentId { get; set; }
        public decimal Score { get; set; }
        public int GradeTypeId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey(nameof(EnrollmentId))]
        public virtual Enrollment Enrollment { get; set; }
        [ForeignKey(nameof(GradeTypeId))]
        public virtual GradeType GradeType { get; set; }
    }
}
