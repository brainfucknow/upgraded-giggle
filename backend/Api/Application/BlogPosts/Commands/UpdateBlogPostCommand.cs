using Api.Application.BlogPosts.DTOs;
using Api.Application.Common.Interfaces;

namespace Api.Application.BlogPosts.Commands;

public class UpdateBlogPostCommand
{
    private readonly IBlogPostRepository _repository;

    public UpdateBlogPostCommand(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<BlogPostDto?> ExecuteAsync(int id, UpdateBlogPostDto updateDto)
    {
        var existingBlogPost = await _repository.GetByIdAsync(id);
        if (existingBlogPost == null)
            return null;

        existingBlogPost.Title = updateDto.Title;
        existingBlogPost.Body = updateDto.Body;

        var updatedBlogPost = await _repository.UpdateAsync(existingBlogPost);

        return new BlogPostDto
        {
            Id = updatedBlogPost.Id,
            Title = updatedBlogPost.Title,
            Body = updatedBlogPost.Body,
            CreatedAt = updatedBlogPost.CreatedAt
        };
    }
}