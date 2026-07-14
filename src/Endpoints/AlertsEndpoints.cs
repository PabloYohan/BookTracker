using BookPromoTracker.Services;

namespace BookPromoTracker.Endpoints;

public static class AlertsEndpoints
{
    public static IEndpointRouteBuilder MapAlertsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/alerts").WithTags("Alerts");

        group.MapGet("/", ListAlerts);
        group.MapGet("/unread-count", GetUnreadCount);
        group.MapPatch("/{id:guid}/read", MarkAsRead);
        group.MapPatch("/{id:guid}/unread", MarkAsUnread);

        return app;
    }

    private static async Task<IResult> ListAlerts(
        bool? unreadOnly,
        Guid? bookId,
        IAlertService alertService,
        CancellationToken cancellationToken
    )
    {
        var alerts = await alertService.ListAsync(unreadOnly, bookId, cancellationToken);
        return Results.Ok(alerts);
    }

    private static async Task<IResult> GetUnreadCount(
        IAlertService alertService,
        CancellationToken cancellationToken
    )
    {
        var count = await alertService.GetUnreadCountAsync(cancellationToken);

        return Results.Ok(new { count });
    }

    private static async Task<IResult> MarkAsRead(
        Guid id,
        IAlertService alertService,
        CancellationToken cancellationToken
    )
    {
        var updated = await alertService.MarkAsReadAsync(id, cancellationToken);

        return updated ? Results.NoContent() : Results.NotFound();
    }

    private static async Task<IResult> MarkAsUnread(
        Guid id,
        IAlertService alertService,
        CancellationToken cancellationToken
    )
    {
        var updated = await alertService.MarkAsUnreadAsync(id, cancellationToken);

        return updated ? Results.NoContent() : Results.NotFound();
    }
}
