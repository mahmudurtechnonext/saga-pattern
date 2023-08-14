using MassTransit;
using Microsoft.EntityFrameworkCore;
using Student.Data;
using Student.StateMachines;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling =
            Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddDbContext<DBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddMassTransit(x =>
{
    const string configurationString = "3.1.78.110:6379,password=foobared";
    x.AddSaga<CreateStudentOrchestrator>()
        .RedisRepository(r => {
            r.DatabaseConfiguration(configurationString);

            // Default is Optimistic
            r.ConcurrencyMode = ConcurrencyMode.Pessimistic;

            // Optional, prefix each saga instance key with the string specified
            // resulting dev:c6cfd285-80b2-4c12-bcd3-56a00d994736
            r.KeyPrefix = "dev";

            // Optional, to customize the lock key
            r.LockSuffix = "-lockage";

            // Optional, the default is 30 seconds
            r.LockTimeout = TimeSpan.FromSeconds(90);
        });


    x.UsingRabbitMq((context, config) =>
    {
        //var connection = new Uri("amqp://admin:admin2023@18.138.164.11:5672");
        var connection = new Uri("amqp://guest:guest@localhost:5672");
        config.Host(connection);

        config.ReceiveEndpoint("student-service-queue", e =>
        {
            e.ConfigureSaga<CreateStudentOrchestrator>(context);
        });
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
