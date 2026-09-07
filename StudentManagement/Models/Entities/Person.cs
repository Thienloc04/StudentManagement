using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models.Entities
{
    public class Person
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; }
        public bool Sex { get; set; } // true = Nam, false = Nữ
        [StringLength(50)]
        public string Phone { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(200)]
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
