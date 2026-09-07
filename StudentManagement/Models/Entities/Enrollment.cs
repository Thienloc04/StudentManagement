using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models.Entities
{
    [Table("Enrollments")]
    public class Enrollment
    {
        [Key]
        public int Id { get; set; }
        public int SubjectClassId { get; set; }
        public int StudentId { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime EndDate { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }
        [ForeignKey(nameof(SubjectClassId))]
        public virtual Subject_Class SubjectClass { get; set; }

    }
}
