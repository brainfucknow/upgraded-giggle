using Microsoft.AspNetCore.Mvc;
using Api.Application.BlogPosts.DTOs;
using Api.Application.BlogPosts.Queries;
using Api.Application.BlogPosts.Commands;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlogPostsController : ControllerBase
{
    private readonly GetAllBlogPostsQuery _getAllQuery;
    private readonly GetBlogPostByIdQuery _getByIdQuery;
    private readonly CreateBlogPostCommand _createCommand;
    private readonly UpdateBlogPostCommand _updateCommand;
    private readonly DeleteBlogPostCommand _deleteCommand;

    public BlogPostsController(
        GetAllBlogPostsQuery getAllQuery,
        GetBlogPostByIdQuery getByIdQuery,
        CreateBlogPostCommand createCommand,
        UpdateBlogPostCommand updateCommand,
        DeleteBlogPostCommand deleteCommand)
    {
        _getAllQuery = getAllQuery;
        _getByIdQuery = getByIdQuery;
        _createCommand = createCommand;
        _updateCommand = updateCommand;
        _deleteCommand = deleteCommand;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BlogPostDto>>> GetBlogPosts()
    {
        var blogPosts = await _getAllQuery.ExecuteAsync();
        return Ok(blogPosts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BlogPostDto>> GetBlogPost(int id)
    {
        var blogPost = await _getByIdQuery.ExecuteAsync(id);

        if (blogPost == null)
        {
            return NotFound();
        }

        return Ok(blogPost);
    }

    [HttpPost]
    public async Task<ActionResult<BlogPostDto>> CreateBlogPost(CreateBlogPostDto createDto)
    {
        var blogPost = await _createCommand.ExecuteAsync(createDto);
        return CreatedAtAction(nameof(GetBlogPost), new { id = blogPost.Id }, blogPost);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BlogPostDto>> UpdateBlogPost(int id, UpdateBlogPostDto updateDto)
    {
        var blogPost = await _updateCommand.ExecuteAsync(id, updateDto);

        if (blogPost == null)
        {
            return NotFound();
        }

        return Ok(blogPost);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBlogPost(int id)
    {
        var deleted = await _deleteCommand.ExecuteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}