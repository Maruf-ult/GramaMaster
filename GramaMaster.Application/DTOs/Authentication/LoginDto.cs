using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Authentication
{
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
