using Docker.Api.Models;
using Microsoft.EntityFrameworkCore;
using Migration.WorkerService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), opt =>
    {
        opt.MigrationsAssembly("Migration.WorkerService");
    });
});
var host = builder.Build();
host.Run();
