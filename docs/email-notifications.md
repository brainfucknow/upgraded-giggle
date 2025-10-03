# Email Notifications for Blog Posts

The application now supports automatic email notifications when new blog posts are created. Here's how to set it up:

## Configuration

### Development Environment
In development, emails are logged to the console instead of being sent. This allows you to test the functionality without needing SMTP configuration.

### Production Environment
To enable actual email sending, configure the following settings in your `appsettings.json` or `appsettings.Production.json`:

```json
{
  "Email": {
    "FromName": "Your Blog Name",
    "FromAddress": "noreply@yourdomain.com",
    "ToName": "Blog Administrator",
    "ToAddress": "admin@yourdomain.com",
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUser": "your-email@gmail.com",
    "SmtpPassword": "your-app-password",
    "UseSsl": true
  }
}
```

## SMTP Providers

### Gmail
1. Enable 2FA on your Google account
2. Generate an App Password for your application
3. Use these settings:
   - SmtpHost: `smtp.gmail.com`
   - SmtpPort: `587`
   - UseSsl: `true`
   - SmtpUser: Your Gmail address
   - SmtpPassword: The generated App Password

### Other Providers
- **Outlook/Hotmail**: `smtp-mail.outlook.com`, port 587
- **SendGrid**: `smtp.sendgrid.net`, port 587
- **Mailgun**: `smtp.mailgun.org`, port 587

## Environment Variables
For security, you can also use environment variables:

```bash
export Email__SmtpHost="smtp.gmail.com"
export Email__SmtpUser="your-email@gmail.com" 
export Email__SmtpPassword="your-app-password"
export Email__ToAddress="admin@yourdomain.com"
```

## Features
- HTML and plain text email templates
- Asynchronous email sending (doesn't block blog post creation)
- Error handling (failed emails won't prevent blog post creation)
- Comprehensive logging

## Testing
When SMTP is not configured, the email content will be logged to the console, allowing you to verify the email format and content during development.

## Security Notes
- Never commit SMTP passwords to version control
- Use App Passwords instead of your main account password
- Consider using environment variables or Azure Key Vault for production secrets