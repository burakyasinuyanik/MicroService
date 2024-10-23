using Microsoft.EntityFrameworkCore;

namespace Docker.Api.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext()
        {
            
        }
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) 
        {
            
        }
        public DbSet<Productcs> Productcs { get; set; }
    }
}
