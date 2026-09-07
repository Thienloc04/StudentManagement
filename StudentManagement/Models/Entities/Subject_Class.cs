using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models.Entities
{
    [Table("Subject_Classes")]
    public class Subject_Class
    {
        [Key]
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int ClassId { get; set; }
        public int TeacherId { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey(nameof(SubjectId))]
        public virtual Subject Subject { get; set; }
        [ForeignKey(nameof(ClassId))]
        public virtual Class Class { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public virtual Teacher Teacher { get; set; } 

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
