namespace Student.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int StudentId { get; set; }
    }
}
