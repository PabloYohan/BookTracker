using BookPromoTracker.Dtos.Books;
using BookPromoTracker.Dtos.Prices;
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
        group.MapPost("/{id:guid}/check-price", CheckBookPriceDeprecated);
        group.MapPost("/{id:guid}/prices", RegisterManualPrice);
        group.MapGet("/{id:guid}/price-history", GetPriceHistory);
        group.MapGet("/{id:guid}/lowest-price", GetLowestPrice);

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

    private static IResult CheckBookPriceDeprecated()
    {
        return Results.Json(
            new
            {
                message =
                    "A consulta automática foi desativada. "
                    + "Registre o preço manualmente em POST /api/books/{id}/prices.",
            },
            statusCode: StatusCodes.Status410Gone
        );
    }

    private static async Task<IResult> RegisterManualPrice(
        Guid id,
        RegisterManualPriceRequest request,
        IPriceHistoryService priceHistoryService,
        CancellationToken cancellationToken
    )
    {
        var (result, errors, bookNotFound, bookInactive) =
            await priceHistoryService.RegisterManualPriceAsync(id, request, cancellationToken);

        if (errors is not null)
        {
            return Results.ValidationProblem(errors);
        }

        if (bookNotFound)
        {
            return Results.NotFound();
        }

        if (bookInactive)
        {
            return Results.Conflict(
                new { message = "Não é possível registrar preço para um livro inativo." }
            );
        }

        return Results.Created($"/api/books/{result!.BookId}/price-history", result);
    }

    private static async Task<IResult> GetPriceHistory(
        Guid id,
        IBookService bookService,
        IPriceHistoryService priceHistoryService,
        CancellationToken cancellationToken
    )
    {
        var book = await bookService.GetByIdAsync(id, cancellationToken);

        if (book is null)
        {
            return Results.NotFound();
        }

        var history = await priceHistoryService.GetPriceHistoryAsync(id, cancellationToken);

        return Results.Ok(history);
    }

    private static async Task<IResult> GetLowestPrice(
        Guid id,
        IPriceHistoryService priceHistoryService,
        CancellationToken cancellationToken
    )
    {
        var (result, bookExists) = await priceHistoryService.GetLowestPriceAsync(
            id,
            cancellationToken
        );

        return bookExists ? Results.Ok(result) : Results.NotFound();
    }
}
