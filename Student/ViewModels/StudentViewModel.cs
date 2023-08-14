namespace Student.ViewModels
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Location { get; set; }
        public ICollection<BookViewModel>? Books { get; set; }
    }
}
