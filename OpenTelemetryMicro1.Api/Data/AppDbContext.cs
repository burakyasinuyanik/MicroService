using Microsoft.EntityFrameworkCore;

namespace OpenTelemetryMicro1.Api.Data
{
    public class AppDbContext(DbContextOptions options):DbContext(options)
    {
       public DbSet<Order> orders { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public string Code { get; set; }
    }
}
