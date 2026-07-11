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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasMany(book => book.PriceHistories)
                .WithOne(history => history.Book)
                .HasForeignKey(history => history.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PriceHistory>(entity =>
        {
            entity.HasIndex(history => history.BookId);
            entity.HasIndex(history => history.CheckedAt);
        });
    }
}
