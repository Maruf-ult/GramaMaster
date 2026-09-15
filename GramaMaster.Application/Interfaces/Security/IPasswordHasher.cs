using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Security
{
    public interface IPasswordHasher
    {
        string Hash(string password);

        bool Verify(string password, string passwordHash);
    }
}
