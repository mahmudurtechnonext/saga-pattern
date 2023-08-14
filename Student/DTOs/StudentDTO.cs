namespace Student.DTOs
{
    public class StudentDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Location { get; set; }
        public ICollection<BookDTO>? Books { get; set; }
    }
}
