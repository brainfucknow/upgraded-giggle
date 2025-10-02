using Api.Application.Common.Interfaces;

namespace Api.Application.BlogPosts.Commands;

public class DeleteBlogPostCommand
{
    private readonly IBlogPostRepository _repository;

    public DeleteBlogPostCommand(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
        var exists = await _repository.ExistsAsync(id);
        if (!exists)
            return false;

        await _repository.DeleteAsync(id);
        return true;
    }
}