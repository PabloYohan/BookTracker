using BookPromoTracker.Data;
using BookPromoTracker.Endpoints;
using BookPromoTracker.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly(
                typeof(AppDbContext).Assembly.FullName
            );
        }
    );
});

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapBooksEndpoints();

app.Run();
