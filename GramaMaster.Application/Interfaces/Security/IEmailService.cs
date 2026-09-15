using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Security
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(
            string email,
            string resetToken);
    }
}
