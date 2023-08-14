using Book.Events;
using MassTransit;
using Student.Data;

namespace Student.StateMachines
{
    public class CreateStudentOrchestrator : ISaga, ISagaVersion, InitiatedBy<CreateBooksEvent>, Orchestrates<CreateBookSuccess>, Orchestrates<CreateBookFailed>
    {
        public Guid CorrelationId { get; set; }
        public string? CurrentState { get; set; }
        public int Version { get; set; }

        public async Task Consume(ConsumeContext<CreateBooksEvent> context)
        {
            var endpoint = await context.GetSendEndpoint(new Uri("queue:book-service-queue"));
            await endpoint.Send(context.Message);
            await Console.Out.WriteLineAsync("Create!");
        }

        public async Task Consume(ConsumeContext<CreateBookSuccess> context)
        {
            await Console.Out.WriteLineAsync("Create Success!");
        }

        public async Task Consume(ConsumeContext<CreateBookFailed> context)
        {
            var _context = context.GetPayload<IServiceProvider>().GetService<DBContext>();
            if (_context == null)
            {
                return;
            }
            var student = await _context.Students!.FindAsync(context.Message.StudentId);
            if (student != null)
            {
                _context.Remove(student);
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
        }
    }
}
