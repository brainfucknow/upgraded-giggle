using Api.Domain.Entities;

namespace Api.Application.Common.Interfaces;

public interface IBlogPostRepository
{
    Task<IEnumerable<BlogPost>> GetAllAsync();
    Task<BlogPost?> GetByIdAsync(int id);
    Task<BlogPost> CreateAsync(BlogPost blogPost);
    Task<BlogPost> UpdateAsync(BlogPost blogPost);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}