using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheGame.Shared;

namespace TheGame.Server.Data
{
    public interface IAuthRepository
    {
        Task<AuthResponse<string>> Login(string usernameOrEmail,string password);
        Task<AuthResponse<int>> Register(User user,string password);
    }
}
