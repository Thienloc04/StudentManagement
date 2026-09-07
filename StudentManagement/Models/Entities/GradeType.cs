using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models.Entities
{
    [Table("Grade_Types")]
    public class GradeType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public TypeEnum Type { get; set; }
        public decimal Weight { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }
    }
}
