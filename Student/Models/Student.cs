using System.ComponentModel.DataAnnotations;

namespace Student.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Location { get; set; }
    }
}
