using Api.Domain.Entities;

namespace Api.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendNewBlogPostNotificationAsync(BlogPost blogPost);
}