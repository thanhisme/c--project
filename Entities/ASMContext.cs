using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Entities
{
    public class ASMContext : DbContext
    {
        public ASMContext() { }

        public ASMContext(DbContextOptions<ASMContext> options) : base(options) { }

        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<RefreshTokenEntity> RefreshToken { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(
                entity =>
                {
                    entity.ToTable("Users");
                    entity.Property(user => user.Id).ValueGeneratedNever();
                }
            );

            modelBuilder.Entity<RefreshTokenEntity>(
                entity =>
                {
                    entity.ToTable("RefreshTokens");
                    entity.HasKey(RT => RT.Id);
                }
            );
        }
    }
}
