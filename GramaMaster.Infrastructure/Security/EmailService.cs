using GramaMaster.Application.Interfaces.Security;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GramaMaster.Infrastructure.Security
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public Task SendPasswordResetEmailAsync(string email, string resetToken)
        {
            _logger.LogInformation("Password reset token for {Email}: {Token}", email, resetToken);
            return Task.CompletedTask;
        }
    }
}
