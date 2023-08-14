using AutoMapper;
using Book.Data;
using Book.Events;
using MassTransit;

namespace Book.Consumers
{
    public class BookConsumer : IConsumer<CreateBooksEvent>
    {
        private readonly DBContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public BookConsumer(DBContext context, IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<CreateBooksEvent> context)
        {
            await Console.Out.WriteLineAsync("Consumed!");
            try
            {
                var books = _mapper.Map<List<Models.Book>>(context.Message.Books);
                await _context.AddRangeAsync(books);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await _publishEndpoint.Publish(new CreateBookFailed
                {
                    CorrelationId = context.Message.CorrelationId,
                    StudentId = context.Message.Books!.ToList().First().StudentId,
                });
                await Console.Out.WriteLineAsync("Exception");
            }
        }
    }
}
