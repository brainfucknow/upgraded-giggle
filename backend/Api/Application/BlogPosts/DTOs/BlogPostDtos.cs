namespace Api.Application.BlogPosts.DTOs;

public class BlogPostDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateBlogPostDto
{
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}

public class UpdateBlogPostDto
{
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}