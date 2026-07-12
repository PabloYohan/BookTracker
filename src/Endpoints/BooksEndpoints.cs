using BookPromoTracker.Dtos.Books;
using BookPromoTracker.Services;

namespace BookPromoTracker.Endpoints;

public static class BooksEndpoints
{
    public static IEndpointRouteBuilder MapBooksEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/books").WithTags("Books");

        group.MapGet("/", ListBooks);
        group.MapGet("/{id:guid}", GetBookById);
        group.MapPost("/", CreateBook);
        group.MapPut("/{id:guid}", UpdateBook);
        group.MapDelete("/{id:guid}", DeleteBook);
        group.MapPatch("/{id:guid}/activate", ActivateMonitoring);
        group.MapPatch("/{id:guid}/deactivate", DeactivateMonitoring);

        return app;
    }

    private static async Task<IResult> ListBooks(
        IBookService bookService,
        CancellationToken cancellationToken
    )
    {
        var books = await bookService.ListAsync(cancellationToken);
        return Results.Ok(books);
    }

    private static async Task<IResult> GetBookById(
        Guid id,
        IBookService bookService,
        CancellationToken cancellationToken
    )
    {
        var book = await bookService.GetByIdAsync(id, cancellationToken);

        return book is null ? Results.NotFound() : Results.Ok(book);
    }

    private static async Task<IResult> CreateBook(
        CreateBookRequest request,
        IBookService bookService,
        CancellationToken cancellationToken
    )
    {
        var (book, errors) = await bookService.CreateAsync(request, cancellationToken);

        if (errors is not null)
        {
            return Results.ValidationProblem(errors);
        }

        return Results.Created($"/api/books/{book!.Id}", book);
    }

    private static async Task<IResult> UpdateBook(
        Guid id,
        UpdateBookRequest request,
        IBookService bookService,
        CancellationToken cancellationToken
    )
    {
        var (book, errors) = await bookService.UpdateAsync(id, request, cancellationToken);

        if (errors is not null)
        {
            return Results.ValidationProblem(errors);
        }

        if (book is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(book);
    }

    private static async Task<IResult> DeleteBook(
        Guid id,
        IBookService bookService,
        CancellationToken cancellationToken
    )
    {
        var deleted = await bookService.DeleteAsync(id, cancellationToken);

        return deleted ? Results.NoContent() : Results.NotFound();
    }

    private static async Task<IResult> ActivateMonitoring(
        Guid id,
        IBookService bookService,
        CancellationToken cancellationToken
    )
    {
        var book = await bookService.SetMonitoringAsync(id, isActive: true, cancellationToken);

        return book is null ? Results.NotFound() : Results.Ok(book);
    }

    private static async Task<IResult> DeactivateMonitoring(
        Guid id,
        IBookService bookService,
        CancellationToken cancellationToken
    )
    {
        var book = await bookService.SetMonitoringAsync(id, isActive: false, cancellationToken);

        return book is null ? Results.NotFound() : Results.Ok(book);
    }
}
