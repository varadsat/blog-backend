using Blog.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BlogDbContext>(options => options.UseSqlite("Data Source=Blog.db"));
var isDevelopment = builder.Environment.IsDevelopment();
builder.Services.AddCors(options =>
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyHeader().AllowAnyMethod().WithOrigins(isDevelopment ? "http://localhost:3000" : "https://thevarad.me").AllowCredentials();
                    }));

var app = builder.Build();

app.UseCors();

app.UseHttpLogging();
app.UseHttpsRedirection();

// TODO: Refactor and add DTOs
// TODO: Refactor to Services and Repositories
// TODO: Write Unit Tests

app.MapGet("/posts", (BlogDbContext context, HttpContext httpContext) =>
{
    return context.Posts.Include(x => x.Author).ToListAsync();
});

app.MapGet("/posts/{postId}", (BlogDbContext context, int postId) =>
{
    return context.Posts.Include(x => x.Author).FirstOrDefaultAsync(p => p.Id == postId);
});

app.MapPost("/addblog", async (BlogDbContext context, Blog.Models.Post post) =>
 {
     await context.Posts.AddAsync(post);
     await context.SaveChangesAsync();
     return Results.Ok();
 });
app.MapPost("/delblog/{id}", async (BlogDbContext context, int id) =>
{
    try
    {
        var itemToRemove = await context.Posts.FirstAsync(x => x.Id == id);
        context.Posts.Remove(itemToRemove);
    }
    catch (InvalidOperationException) { }
    await context.SaveChangesAsync();
    return Results.Ok();
});
app.MapPost("/seedpost", async (BlogDbContext context) =>
 {
     await context.Posts.AddAsync(new Blog.Models.Post { Author = new Blog.Models.Author { Name = "Varad", ImageUrl = "sample.png" }, Body = "Lorem Ipsum è un testo segnaposto utilizzato nel settore della tipografia e della stampa. Lorem Ipsum è considerato il testo segnaposto standard sin dal sedicesimo secolo, quando un anonimo tipografo prese una cassetta di caratteri e li assemblò per preparare un testo campione.Lorem Ipsum è un testo segnaposto utilizzato nel settore della tipografia e della stampa. Lorem Ipsum è considerato il testo segnaposto standard sin dal sedicesimo secolo, quando un anonimo tipografo prese una cassetta di caratteri e li assemblò per preparare un testo campione.", PublishedAt = DateTime.UtcNow, Tags = new string[] { "hello", "world" }, Title = "Sqlite" });
     await context.SaveChangesAsync();
     return Results.Ok();
 });

using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetRequiredService<BlogDbContext>())
{
    await context.Database.MigrateAsync();
}

app.Run();
