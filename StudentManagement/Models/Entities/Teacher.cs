using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models.Entities
{
    [Table("Teachers")]
    public class Teacher : Person
    {
        public string TeacherNo { get; set; }
        public int Status { get; set; }

        public virtual ICollection<Subject_Class> SubjectClasses { get; set; }
    }
}
