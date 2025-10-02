using Microsoft.EntityFrameworkCore;
using Api.Domain.Entities;
using Api.Infrastructure.Data;
using Api.Infrastructure.Repositories;
using Api.Application.BlogPosts.Commands;
using Api.Application.BlogPosts.Queries;
using Api.Application.BlogPosts.DTOs;

namespace Tests;

public class BlogPostCrudTests
{
    private ApplicationDbContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task CreateBlogPost_Should_Return_Created_BlogPost()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var repository = new BlogPostRepository(context);
        var command = new CreateBlogPostCommand(repository);
        var createDto = new CreateBlogPostDto
        {
            Title = "Test Title",
            Body = "Test Body"
        };

        // Act
        var result = await command.ExecuteAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Title", result.Title);
        Assert.Equal("Test Body", result.Body);
        Assert.True(result.Id > 0);
        Assert.True(result.CreatedAt > DateTime.MinValue);
    }

    [Fact]
    public async Task GetAllBlogPosts_Should_Return_All_BlogPosts()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var repository = new BlogPostRepository(context);
        var query = new GetAllBlogPostsQuery(repository);

        // Seed data
        context.BlogPosts.AddRange(
            new BlogPost { Title = "Post 1", Body = "Body 1", CreatedAt = DateTime.UtcNow },
            new BlogPost { Title = "Post 2", Body = "Body 2", CreatedAt = DateTime.UtcNow.AddMinutes(-1) }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await query.ExecuteAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        // Should be ordered by CreatedAt descending (Post 1 first)
        Assert.Equal("Post 1", result.First().Title);
    }

    [Fact]
    public async Task GetBlogPostById_Should_Return_Correct_BlogPost()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var repository = new BlogPostRepository(context);
        var query = new GetBlogPostByIdQuery(repository);

        var blogPost = new BlogPost { Title = "Test Post", Body = "Test Body", CreatedAt = DateTime.UtcNow };
        context.BlogPosts.Add(blogPost);
        await context.SaveChangesAsync();

        // Act
        var result = await query.ExecuteAsync(blogPost.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Post", result.Title);
        Assert.Equal("Test Body", result.Body);
        Assert.Equal(blogPost.Id, result.Id);
    }

    [Fact]
    public async Task GetBlogPostById_Should_Return_Null_When_Not_Found()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var repository = new BlogPostRepository(context);
        var query = new GetBlogPostByIdQuery(repository);

        // Act
        var result = await query.ExecuteAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateBlogPost_Should_Update_Existing_BlogPost()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var repository = new BlogPostRepository(context);
        var command = new UpdateBlogPostCommand(repository);

        var blogPost = new BlogPost { Title = "Original Title", Body = "Original Body", CreatedAt = DateTime.UtcNow };
        context.BlogPosts.Add(blogPost);
        await context.SaveChangesAsync();

        var updateDto = new UpdateBlogPostDto
        {
            Title = "Updated Title",
            Body = "Updated Body"
        };

        // Act
        var result = await command.ExecuteAsync(blogPost.Id, updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Title", result.Title);
        Assert.Equal("Updated Body", result.Body);
        Assert.Equal(blogPost.Id, result.Id);
        Assert.Equal(blogPost.CreatedAt, result.CreatedAt); // CreatedAt should not change
    }

    [Fact]
    public async Task UpdateBlogPost_Should_Return_Null_When_Not_Found()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var repository = new BlogPostRepository(context);
        var command = new UpdateBlogPostCommand(repository);

        var updateDto = new UpdateBlogPostDto
        {
            Title = "Updated Title",
            Body = "Updated Body"
        };

        // Act
        var result = await command.ExecuteAsync(999, updateDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteBlogPost_Should_Delete_Existing_BlogPost()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var repository = new BlogPostRepository(context);
        var command = new DeleteBlogPostCommand(repository);

        var blogPost = new BlogPost { Title = "Test Post", Body = "Test Body", CreatedAt = DateTime.UtcNow };
        context.BlogPosts.Add(blogPost);
        await context.SaveChangesAsync();

        // Act
        var result = await command.ExecuteAsync(blogPost.Id);

        // Assert
        Assert.True(result);
        Assert.False(await context.BlogPosts.AnyAsync(bp => bp.Id == blogPost.Id));
    }

    [Fact]
    public async Task DeleteBlogPost_Should_Return_False_When_Not_Found()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var repository = new BlogPostRepository(context);
        var command = new DeleteBlogPostCommand(repository);

        // Act
        var result = await command.ExecuteAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Repository_ExistsAsync_Should_Return_True_When_BlogPost_Exists()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var repository = new BlogPostRepository(context);

        var blogPost = new BlogPost { Title = "Test Post", Body = "Test Body", CreatedAt = DateTime.UtcNow };
        context.BlogPosts.Add(blogPost);
        await context.SaveChangesAsync();

        // Act
        var exists = await repository.ExistsAsync(blogPost.Id);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task Repository_ExistsAsync_Should_Return_False_When_BlogPost_DoesNot_Exist()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var repository = new BlogPostRepository(context);

        // Act
        var exists = await repository.ExistsAsync(999);

        // Assert
        Assert.False(exists);
    }
}