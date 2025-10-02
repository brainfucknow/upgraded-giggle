using Microsoft.EntityFrameworkCore;
using Api.Infrastructure.Data;
using Api.Application.Common.Interfaces;
using Api.Infrastructure.Repositories;
using Api.Application.BlogPosts.Queries;
using Api.Application.BlogPosts.Commands;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Frontend URL
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure Entity Framework with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=blog.db"));

// Register repositories
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();

// Register use cases
builder.Services.AddScoped<GetAllBlogPostsQuery>();
builder.Services.AddScoped<GetBlogPostByIdQuery>();
builder.Services.AddScoped<CreateBlogPostCommand>();
builder.Services.AddScoped<UpdateBlogPostCommand>();
builder.Services.AddScoped<DeleteBlogPostCommand>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowFrontend");

app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();

// Make Program accessible for testing
public partial class Program { }
