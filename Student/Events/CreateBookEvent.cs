using MassTransit;
using Student.DTOs;

namespace Book.Events
{
    public class CreateBookEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public BookDTO? Book { get; set; }
    }

    public class CreateBooksEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public List<BookDTO>? Books { get; set; }
    }

    public class CreateBookSuccess : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public int StudentId { get; set; }
    }

    public class CreateBookFailed : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public int StudentId { get; set; }
    }
}
