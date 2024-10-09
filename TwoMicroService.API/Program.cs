using Bus.Shared;
using MassTransit;
using TwoMicroService.API.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//mastrqansit configure
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserCretedEventConsumer>();
    x.UsingRabbitMq((context, configure) =>
    {
        var connectionString = builder.Configuration.GetConnectionString("RabbitMQ");
        configure.Host(connectionString);
        //microservice.queueName.document kuyruk oluþturma iþlemi
        configure.ReceiveEndpoint("email-microservice.user-created.event.queue", c =>
        {
            c.ConfigureConsumer<UserCretedEventConsumer>(context);
        });

    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
