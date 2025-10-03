using System.Text;
using System.Text.Json;
using Api.Application.Common.Interfaces;
using Api.Domain.Entities;

namespace Api.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;
    private readonly HttpClient _httpClient;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger, HttpClient httpClient)
    {
        _configuration = configuration;
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task SendNewBlogPostNotificationAsync(BlogPost blogPost)
    {
        try
        {
            var mailinatorApiKey = _configuration["Mailinator:ApiKey"];
            var mailinatorDomain = _configuration["Mailinator:Domain"] ?? "mailinator.com";
            var toEmail = _configuration["Email:ToAddress"] ?? "testblog@mailinator.com";

            // Extract the username from the email address (part before @)
            var username = toEmail.Split('@')[0];
            
            // Create email content
            var subject = $"New Blog Post Created: {blogPost.Title}";
            var htmlBody = $@"
                <h2>New Blog Post Created</h2>
                <p>A new blog post has been created on your blog:</p>
                <h3>{blogPost.Title}</h3>
                <p><strong>Created:</strong> {blogPost.CreatedAt:yyyy-MM-dd HH:mm:ss}</p>
                <div>
                    <h4>Content:</h4>
                    <div style='border: 1px solid #ddd; padding: 10px; background-color: #f9f9f9;'>
                        {System.Net.WebUtility.HtmlEncode(blogPost.Body)}
                    </div>
                </div>
                <p>Best regards,<br/>Your Blog Application</p>";

            var textBody = $@"
New Blog Post Created

A new blog post has been created on your blog:

Title: {blogPost.Title}
Created: {blogPost.CreatedAt:yyyy-MM-dd HH:mm:ss}

Content:
{blogPost.Body}

Best regards,
Your Blog Application";

            if (string.IsNullOrEmpty(mailinatorApiKey))
            {
                // Development mode - just log the email
                _logger.LogInformation("Email would be sent to Mailinator (API key not configured):\nTo: {To}\nSubject: {Subject}\nInbox URL: https://www.mailinator.com/v4/public/inboxes.jsp?to={Username}", 
                    toEmail, subject, username);
                _logger.LogInformation("Email Body:\n{Body}", textBody);
                return;
            }

            // Create message data for Mailinator injection API
            var messageData = new
            {
                subject = subject,
                from = _configuration["Email:FromAddress"] ?? "noreply@blog.com",
                parts = new object[]
                {
                    new { headers = new { }, body = textBody },
                    new { headers = new { content_type = "text/html" }, body = htmlBody }
                }
            };

            // Use Mailinator's message injection API
            var apiUrl = $"https://api.mailinator.com/api/v2/domains/{mailinatorDomain}/inboxes/{username}/messages";
            
            var json = JsonSerializer.Serialize(messageData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            // Add API key to headers
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", mailinatorApiKey);
            
            var response = await _httpClient.PostAsync(apiUrl, content);
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Email sent successfully via Mailinator for new blog post: {Title} to {Email}", 
                    blogPost.Title, toEmail);
                _logger.LogInformation("Check inbox at: https://www.mailinator.com/v4/public/inboxes.jsp?to={Username}", username);
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Failed to send email via Mailinator. Status: {Status}, Response: {Response}",
                    response.StatusCode, responseContent);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email notification via Mailinator for blog post: {Title}", blogPost.Title);
            // Don't throw the exception to prevent blog post creation from failing due to email issues
        }
    }
}