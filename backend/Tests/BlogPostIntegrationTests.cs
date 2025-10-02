using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text;
using Api.Infrastructure.Data;
using Api.Application.BlogPosts.DTOs;

namespace Tests;

public class BlogPostIntegrationTests
{
    private WebApplicationFactory<Program> CreateFactory()
    {
        return new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the existing DbContext registration
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    // Add in-memory database for testing with unique name
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase(Guid.NewGuid().ToString());
                    });
                });
            });
    }

    [Fact]
    public async Task GetBlogPosts_Should_Return_Empty_List_Initially()
    {
        // Arrange
        using var factory = CreateFactory();
        using var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/blogposts");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var blogPosts = JsonSerializer.Deserialize<BlogPostDto[]>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        Assert.NotNull(blogPosts);
        Assert.Empty(blogPosts);
    }

    [Fact]
    public async Task CreateBlogPost_Should_Create_And_Return_BlogPost()
    {
        // Arrange
        using var factory = CreateFactory();
        using var client = factory.CreateClient();
        
        var createDto = new CreateBlogPostDto
        {
            Title = "Integration Test Post",
            Body = "This is a test post created via API"
        };
        
        var json = JsonSerializer.Serialize(createDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/api/blogposts", content);
        
        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var createdPost = JsonSerializer.Deserialize<BlogPostDto>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        Assert.NotNull(createdPost);
        Assert.Equal("Integration Test Post", createdPost.Title);
        Assert.Equal("This is a test post created via API", createdPost.Body);
        Assert.True(createdPost.Id > 0);
    }
}