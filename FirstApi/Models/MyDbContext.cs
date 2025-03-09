using Microsoft.EntityFrameworkCore;

namespace FirstApi.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ActiveToken> ActiveTokens { get; set; } // Renamed for convention
    }
}
