using Microsoft.EntityFrameworkCore;
using ProductDesk.Models;

namespace ProductDesk.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set;  }
    }
}
