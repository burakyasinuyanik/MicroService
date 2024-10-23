using Docker.Api.Models;
using Docker.API.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<StockService>(x =>
{
    x.BaseAddress = new Uri(builder.Configuration.GetSection("MicroServices").GetSection("StockService")["BaseUrl"]);
}).AddPolicyHandler(AddRetryPolicy())
.AddPolicyHandler(AddCircuitBreakerPolicy())
.AddPolicyHandler(AddTimeOutPolicy());

static AsyncRetryPolicy<HttpResponseMessage> AddRetryPolicy()
{
    var result =HttpPolicyExtensions.HandleTransientHttpError()
        .WaitAndRetryAsync(3,retry=>TimeSpan.FromSeconds(Math.Pow(3,retry)));
    return result;
}
static AsyncCircuitBreakerPolicy<HttpResponseMessage> AddCircuitBreakerPolicy()
{
    var result = HttpPolicyExtensions.HandleTransientHttpError()
        .CircuitBreakerAsync(3, TimeSpan.FromSeconds(15));
    return result;
}
static AsyncTimeoutPolicy<HttpResponseMessage> AddTimeOutPolicy()
{
    var result = Policy.TimeoutAsync<HttpResponseMessage>(2);
    return result;
}
var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var dbContext= scope.ServiceProvider.GetService<AppDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();

app.MapControllers();

app.Run();
