using Api.Application.BlogPosts.DTOs;
using Api.Application.Common.Interfaces;

namespace Api.Application.BlogPosts.Queries;

public class GetAllBlogPostsQuery
{
    private readonly IBlogPostRepository _repository;

    public GetAllBlogPostsQuery(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<BlogPostDto>> ExecuteAsync()
    {
        var blogPosts = await _repository.GetAllAsync();
        return blogPosts.Select(bp => new BlogPostDto
        {
            Id = bp.Id,
            Title = bp.Title,
            Body = bp.Body,
            CreatedAt = bp.CreatedAt
        });
    }
}