using Bus.Shared;
using MassTransit;
using TwoMicroService.API;
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
        //configure.UseMessageRetry(r => r.Immediate(5)); //retry adeti
        //  configure.UseMessageRetry(r=>r.Interval(5,TimeSpan.FromSeconds(5))); 5 saniye aral�kla
        //configure.UseMessageRetry(r=>r.Incremental(5,TimeSpan.FromSeconds(5),TimeSpan.FromSeconds(5)));//5 saniye ekleyerek

        configure.UseMessageRetry(x =>
        {
            x.Interval(5,TimeSpan.FromSeconds(5));

            //x.Handle<QueueCriticalException>(); 
            x.Ignore<QueueNormalException>(); //i�leme ve retry olmas�n

        });

        configure.UseDelayedRedelivery(x => x.Intervals(TimeSpan.FromHours(1), TimeSpan.FromHours(2)));
        configure.PrefetchCount = 10;//
        configure.ConcurrentMessageLimit = 10;//mesajlar� ayn� anda i�leme
        configure.UseInMemoryOutbox(context);//bir hata ile kar��la����nda 

        var connectionString = builder.Configuration.GetConnectionString("RabbitMQ");
        configure.Host(connectionString);
        //microservice.queueName.document kuyruk olu�turma i�lemi
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
