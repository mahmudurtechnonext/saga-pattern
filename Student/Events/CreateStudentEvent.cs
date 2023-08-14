using MassTransit;
using Student.DTOs;
using Student.ViewModels;

namespace Student.Events
{
    public class CreateStudentEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public StudentDTO? Student { get; set; }
    }

    public class CreateStudentSuccess : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public int StudentId { get; set; }
    }

    public class CreateStudentFailed : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set;}
        public int StudentId { get; set; }
    }
}
