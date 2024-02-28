using Microsoft.EntityFrameworkCore;

namespace Exercise2.Controllers;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Weather> Weather { get; set; }
}
