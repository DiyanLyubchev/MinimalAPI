using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MinimalBookAPI.Context;
using MinimalBookAPI.DbModels;
using MinimalBookAPI.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", a => a.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});

//DB context setup
var binder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false);
var configuration = binder.Build();
builder.Services.AddDbContext<BookContext>(options =>
options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//Use Cors
app.UseCors("AllowAll");

app.MapGet("/book/all", async (BookContext context) =>
{
    var books = await context.Books.AsNoTracking().ToListAsync();
    return books;
});

app.MapGet("/book/by-id/{id}", async ([FromRoute] int id, BookContext context) =>
{
    return await context.Books.FindAsync(id)
              is Book book
              ? Results.Ok(book)
              : Results.NotFound();
});

app.MapGet("/book/by-name/{name}", async ([FromRoute] string name, BookContext context) =>
{
    return await context.Books.AsNoTracking().Where(book => book.Name.Contains(name)).ToListAsync();
});

app.MapPost("/book", async ([FromBody] BookViewModel bookVm, BookContext context) =>
{
    Book book = new()
    {
        Name = bookVm.Name,
        Page = bookVm.Page,
    };
    await context.Books.AddAsync(book);
    await context.SaveChangesAsync();

    return Results.Ok(book);
});


app.MapPut("/book/{id}", async ([FromRoute] int id, [FromBody] Book book, BookContext context) =>
{
    Book entity = await context.Books.FindAsync(id);

    if (entity is null)
        return Results.NotFound();

    entity.Name = book.Name;
    entity.Page = book.Page;
    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/book/{id}", async ([FromRoute] int id, BookContext context) =>
{
    Book entity = await context.Books.FindAsync(id);

    if (entity is null)
        return Results.NotFound();

    context.Remove(entity);
    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();
