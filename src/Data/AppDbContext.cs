using BookPromoTracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookPromoTracker.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<PriceHistory> PriceHistories => Set<PriceHistory>();
    public DbSet<Alert> Alerts => Set<Alert>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasMany(book => book.PriceHistories)
                .WithOne(history => history.Book)
                .HasForeignKey(history => history.BookId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(book => book.Alerts)
                .WithOne(alert => alert.Book)
                .HasForeignKey(alert => alert.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PriceHistory>(entity =>
        {
            entity.HasIndex(history => history.BookId);
            entity.HasIndex(history => history.CheckedAt);
        });

        modelBuilder.Entity<Alert>(entity =>
        {
            entity.HasIndex(alert => alert.BookId);
            entity.HasIndex(alert => alert.CreatedAt);
        });
    }
}
