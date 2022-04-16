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

app.MapGet("/posts",async(PostRepository repo) =>
{
    var posts =await repo.GetAllPosts();
    return Results.Ok(posts);
});

app.MapGet("/posts/{postId}", async (PostRepository repo, int postId) =>
{
    var post = await repo.GetPostById(postId);
    if(post is not null)
        return Results.Ok(post);
    return Results.NotFound();
});

app.MapPost("/addblog",async (PostRepository repo, Blog.Models.Post post) =>
 {
     await repo.AddPost(post);
     return Results.Ok();
 });
app.MapPost("/delblog/{id}", async (PostRepository repo, int id) =>
{ 
    var deletedPost = await repo.DeletePost(id);     
    return deletedPost != null ? Results.Ok() : Results.NotFound();
});

using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetRequiredService<BlogDbContext>())
{
    await context.Database.MigrateAsync();
}

app.Run();
