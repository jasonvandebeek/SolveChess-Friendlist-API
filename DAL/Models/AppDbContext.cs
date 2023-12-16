
using Microsoft.EntityFrameworkCore;

namespace SolveChess.DAL.Models;

public class AppDbContext : DbContext
{

    public DbSet<RequestModel> Request { get; set; }
    public DbSet<FriendModel> Friend { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RequestModel>()
            .HasKey(r => new { r.UserId, r.SenderId });

        modelBuilder.Entity<FriendModel>()
            .HasKey(r => new { r.UserId, r.FriendId });
    }

}