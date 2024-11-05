using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetryMicro1.Api;
using OpenTelemetryMicro1.Api.Data;
using OpenTelemetryMicro1.Api.Services;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<Micro2Service>();
builder.Services.AddDbContext<AppDbContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddOpenTelemetry().WithTracing(o =>
{   //docker-compose -f docker-compose.telemetry.yml up
    //docker run --rm --name jaeger -p 4317:4317 -p 16686:16686 jaegertracing/all-in-one
    //uygulama performans� k���k �rneklem ile stabiletityi yakalama
    //o.SetSampler(new TraceIdRatioBasedSampler(2));
    // o.SetSampler(new AlwaysOnSampler());
    o.ConfigureResource(x => x.AddService("order.api", "1.0v"));

    o.AddSource("OpenTelemetryMicro1.Api.Source");
    o.AddAspNetCoreInstrumentation(o =>
    {
        o.RecordException = true;



        o.EnrichWithHttpRequest = (activity, request) =>
        {
            var userId = 200;
            activity.AddTag("userId", 200);
        };


        o.Filter = (context =>
        {
            return context.Request.Path.Value.Contains("api");
        });
    });
    o.AddEntityFrameworkCoreInstrumentation(o =>
    {
        o.EnrichWithIDbCommand = (activity, command) =>
        {
            activity.AddTag("commandText", command.CommandText);
        };
        o.SetDbStatementForStoredProcedure = true;
        o.SetDbStatementForText = true;

    });
    o.AddHttpClientInstrumentation();
    o.AddConsoleExporter();

    o.AddOtlpExporter();
    //docker run --rm --name jaeger -p 4317:4317 -p 16686:16686 jaegertracing/all-in-one
});
//    .WithLogging(o =>
//{
//    o.ConfigureResource(x => x.AddService("order.api", "1.0v"));

//    o.AddConsoleExporter();

//    o.AddOtlpExporter();
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("api/order", async (ILogger<Program> logger, AppDbContext context,Micro2Service micro2Service) =>
{


    var response = await micro2Service.GetMicro2Data();

    context.orders.Add(new Order { Code = "123" });
    await context.SaveChangesAsync();
    Activity.Current?.AddTag("orderCode", "123");


    using (var activity = ActivitySourceProvider.Instance.StartActivity("File yazma operasyonu", ActivityKind.Server))
    {
        var a = "Merhaba D�nya";
        File.WriteAllText("Example.txt",a);

    }

    var userId = 99;
    logger.LogInformation("sipari� end point �al��t�r");
    logger.LogInformation("Sipari� olu�tu,userId={userId}", userId);

    return Results.Ok(response);
});

app.MapGet("/weatherforecast", () =>
{


    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
