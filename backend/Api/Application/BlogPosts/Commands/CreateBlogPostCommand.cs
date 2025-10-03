using Api.Application.BlogPosts.DTOs;
using Api.Application.Common.Interfaces;
using Api.Domain.Entities;

namespace Api.Application.BlogPosts.Commands;

public class CreateBlogPostCommand
{
    private readonly IBlogPostRepository _repository;
    private readonly IEmailService _emailService;

    public CreateBlogPostCommand(IBlogPostRepository repository, IEmailService emailService)
    {
        _repository = repository;
        _emailService = emailService;
    }

    public async Task<BlogPostDto> ExecuteAsync(CreateBlogPostDto createDto)
    {
        var blogPost = new BlogPost
        {
            Title = createDto.Title,
            Body = createDto.Body
        };

        var createdBlogPost = await _repository.CreateAsync(blogPost);

        // Send email notification asynchronously (fire and forget)
        _ = Task.Run(async () => await _emailService.SendNewBlogPostNotificationAsync(createdBlogPost));

        return new BlogPostDto
        {
            Id = createdBlogPost.Id,
            Title = createdBlogPost.Title,
            Body = createdBlogPost.Body,
            CreatedAt = createdBlogPost.CreatedAt
        };
    }
}