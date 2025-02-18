using Microsoft.EntityFrameworkCore;

namespace FirstApi.Models
{
    public class MyDbContext : DbContext
    {
        // public MyDbContext()
        // {
        // }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("t_users");

                entity.Property(e => e.UserId).UseSerialColumn();//Use SERIAL instead of GENERATED AS IDENTITY
            });
        }

    }

}

