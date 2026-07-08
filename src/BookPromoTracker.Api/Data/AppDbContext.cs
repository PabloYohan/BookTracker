using BookPromoTracker.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookPromoTracker.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();
}
