using Blog.Data;
using Blog.Models;
using Blog.Models.DTOs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BlogDbContext>(options => options.UseSqlite("Data Source=Blog.db"));
builder.Services.AddScoped<PostRepository>();
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

app.MapGet("/posts",(PostRepository repo) =>
{
    var posts = repo.GetAllPosts();
    return Results.Ok(posts.Result);
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
app.MapDelete("/delblog/{id}", async (BlogDbContext context, int id) =>
{
  
    var itemToRemove = await context.Posts.FirstOrDefaultAsync(x => x.Id == id);
    if (itemToRemove != null)
    {
        context.Posts.Remove(itemToRemove);
        await context.SaveChangesAsync();
    }      
    return Results.Ok();
});

using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetRequiredService<BlogDbContext>())
{
    await context.Database.MigrateAsync();
}

app.Run();
