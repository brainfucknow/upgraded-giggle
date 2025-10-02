using Api.Application.BlogPosts.DTOs;
using Api.Application.Common.Interfaces;
using Api.Domain.Entities;

namespace Api.Application.BlogPosts.Commands;

public class CreateBlogPostCommand
{
    private readonly IBlogPostRepository _repository;

    public CreateBlogPostCommand(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<BlogPostDto> ExecuteAsync(CreateBlogPostDto createDto)
    {
        var blogPost = new BlogPost
        {
            Title = createDto.Title,
            Body = createDto.Body
        };

        var createdBlogPost = await _repository.CreateAsync(blogPost);

        return new BlogPostDto
        {
            Id = createdBlogPost.Id,
            Title = createdBlogPost.Title,
            Body = createdBlogPost.Body,
            CreatedAt = createdBlogPost.CreatedAt
        };
    }
}