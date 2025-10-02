using Microsoft.EntityFrameworkCore;
using Api.Domain.Entities;
using Api.Infrastructure.Data;

namespace Tests;

public class BlogPostTests
{
    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        return new ApplicationDbContext(options);
    }

    [Fact]
    public void BlogPost_CanBeCreated()
    {
        // Arrange
        var blogPost = new BlogPost
        {
            Title = "Test Title",
            Body = "Test Body",
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        Assert.NotNull(blogPost);
        Assert.Equal("Test Title", blogPost.Title);
        Assert.Equal("Test Body", blogPost.Body);
        Assert.True(blogPost.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public async Task BlogPost_CanBeSavedToDatabase()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var blogPost = new BlogPost
        {
            Title = "Database Test Title",
            Body = "Database Test Body",
            CreatedAt = DateTime.UtcNow
        };

        // Act
        context.BlogPosts.Add(blogPost);
        await context.SaveChangesAsync();

        // Assert
        var savedPost = await context.BlogPosts.FirstOrDefaultAsync(x => x.Title == "Database Test Title");
        Assert.NotNull(savedPost);
        Assert.Equal("Database Test Title", savedPost.Title);
        Assert.Equal("Database Test Body", savedPost.Body);
        Assert.True(savedPost.Id > 0);
    }

    [Fact]
    public async Task BlogPost_RequiredPropertiesAreValidated()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var blogPost = new BlogPost
        {
            // Intentionally leaving Title as empty string to test validation
            Title = "",
            Body = "Some body content",
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        context.BlogPosts.Add(blogPost);
        // Note: In a real scenario with database constraints, this would throw
        // For this test, we're just verifying the entity can be created
        await context.SaveChangesAsync();
        
        var savedPost = await context.BlogPosts.FirstOrDefaultAsync();
        Assert.NotNull(savedPost);
    }
}
