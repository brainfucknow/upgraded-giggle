using Microsoft.EntityFrameworkCore;
using Api.Application.Common.Interfaces;
using Api.Domain.Entities;
using Api.Infrastructure.Data;

namespace Api.Infrastructure.Repositories;

public class BlogPostRepository : IBlogPostRepository
{
    private readonly ApplicationDbContext _context;

    public BlogPostRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BlogPost>> GetAllAsync()
    {
        return await _context.BlogPosts
            .OrderByDescending(bp => bp.CreatedAt)
            .ToListAsync();
    }

    public async Task<BlogPost?> GetByIdAsync(int id)
    {
        return await _context.BlogPosts.FindAsync(id);
    }

    public async Task<BlogPost> CreateAsync(BlogPost blogPost)
    {
        blogPost.CreatedAt = DateTime.UtcNow;
        _context.BlogPosts.Add(blogPost);
        await _context.SaveChangesAsync();
        return blogPost;
    }

    public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
    {
        _context.Entry(blogPost).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return blogPost;
    }

    public async Task DeleteAsync(int id)
    {
        var blogPost = await _context.BlogPosts.FindAsync(id);
        if (blogPost != null)
        {
            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.BlogPosts.AnyAsync(bp => bp.Id == id);
    }
}