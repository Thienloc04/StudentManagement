using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models.Entities
{
    [Table("Classes")]
    public class Class
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string ClassNo { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int Status { get; set; }

        public virtual ICollection<Student_Class> StudentClasses { get; set; }

    }
}
