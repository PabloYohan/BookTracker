using BookPromoTracker.Api.Data;
using BookPromoTracker.Api.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/books", async (AppDbContext db) =>
{
    var books = await db.Books
        .OrderBy(book => book.Title)
        .ToListAsync();

    return Results.Ok(books);
});

app.MapGet("/api/books/{id:guid}", async (Guid id, AppDbContext db) =>
{
    var book = await db.Books.FindAsync(id);

    return book is null
        ? Results.NotFound()
        : Results.Ok(book);
});

app.MapPost("/api/books", async (Book book, AppDbContext db) =>
{
    book.Id = Guid.NewGuid();
    book.CreatedAt = DateTime.UtcNow;
    book.IsActive = true;

    db.Books.Add(book);
    await db.SaveChangesAsync();

    return Results.Created($"/api/books/{book.Id}", book);
});

app.MapPut("/api/books/{id:guid}", async (Guid id, Book input, AppDbContext db) =>
{
    var book = await db.Books.FindAsync(id);

    if (book is null)
    {
        return Results.NotFound();
    }

    book.Title = input.Title;
    book.Author = input.Author;
    book.Isbn = input.Isbn;
    book.ProductUrl = input.ProductUrl;
    book.Asin = input.Asin;
    book.TargetPrice = input.TargetPrice;
    book.IsActive = input.IsActive;

    await db.SaveChangesAsync();

    return Results.Ok(book);
});

app.MapDelete("/api/books/{id:guid}", async (Guid id, AppDbContext db) =>
{
    var book = await db.Books.FindAsync(id);

    if (book is null)
    {
        return Results.NotFound();
    }

    db.Books.Remove(book);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();