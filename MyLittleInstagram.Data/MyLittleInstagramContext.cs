using Microsoft.EntityFrameworkCore;
using MyLittleInstagram.Data.Entities;

namespace MyLittleInstagram.Data;

public class MyLittleInstagramContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Friendship> Friendships { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public MyLittleInstagramContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Friendship>()
            .HasKey(f => new { f.User1Id, f.User2Id });

        modelBuilder.Entity<Friendship>()
            .HasOne(f => f.User1)
            .WithMany(u => u.Friendships)
            .HasForeignKey(f => f.User1Id);

        modelBuilder.Entity<Friendship>()
            .HasOne(f => f.User2)
            .WithMany()
            .HasForeignKey(f => f.User2Id);
    }
}