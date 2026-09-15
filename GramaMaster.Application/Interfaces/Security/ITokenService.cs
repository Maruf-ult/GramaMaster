using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Security
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);

        string GenerateRefreshToken();

        DateTime GetRefreshTokenExpiry();

        Guid? GetUserIdFromExpiredToken(string token);
    }
}
