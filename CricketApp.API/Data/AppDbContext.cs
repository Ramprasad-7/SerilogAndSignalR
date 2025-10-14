using CricketApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CricketApp.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Player> Players { get; set; }
    }
}
