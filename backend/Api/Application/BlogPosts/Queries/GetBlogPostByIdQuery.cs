using Api.Application.BlogPosts.DTOs;
using Api.Application.Common.Interfaces;

namespace Api.Application.BlogPosts.Queries;

public class GetBlogPostByIdQuery
{
    private readonly IBlogPostRepository _repository;

    public GetBlogPostByIdQuery(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<BlogPostDto?> ExecuteAsync(int id)
    {
        var blogPost = await _repository.GetByIdAsync(id);
        if (blogPost == null)
            return null;

        return new BlogPostDto
        {
            Id = blogPost.Id,
            Title = blogPost.Title,
            Body = blogPost.Body,
            CreatedAt = blogPost.CreatedAt
        };
    }
}