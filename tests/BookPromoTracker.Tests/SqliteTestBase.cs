using BookPromoTracker.Data;
using BookPromoTracker.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BookPromoTracker.Tests;

public abstract class SqliteTestBase : IDisposable
{
    private readonly SqliteConnection _connection;

    protected SqliteTestBase()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        Db = CreateContext();
        Db.Database.EnsureCreated();
    }

    protected AppDbContext Db { get; }

    protected static AppDbContext CreateContext(SqliteConnection? connection = null)
    {
        var sqliteConnection = connection ?? new SqliteConnection("DataSource=:memory:");
        sqliteConnection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(sqliteConnection)
            .Options;

        return new AppDbContext(options);
    }

    protected Book CreateBook(
        decimal targetPrice = 35m,
        bool isActive = true,
        string title = "O Iluminado"
    )
    {
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = title,
            Author = "Stephen King",
            Isbn = string.Empty,
            Asin = "8532520709",
            ProductUrl = "https://www.amazon.com.br/dp/8532520709",
            TargetPrice = targetPrice,
            IsActive = isActive,
            CreatedAt = DateTime.UtcNow,
        };

        Db.Books.Add(book);
        Db.SaveChanges();

        return book;
    }

    public void Dispose()
    {
        Db.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
