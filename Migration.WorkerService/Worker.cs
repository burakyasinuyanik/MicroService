using Docker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Migration.WorkerService
{
    public class Worker(IServiceProvider service) : BackgroundService
    {
     
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using(var scope = service.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();
                Console.WriteLine("database güncellendi");
            }
        }
    }
}
