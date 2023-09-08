using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheGame.Shared;

namespace TheGame.Client.Services
{
    public interface IAuthService
    {
        Task<AuthResponse<string>> Login(LoginData data);
        Task<AuthResponse<int>> Register(RegisterData data);
    }
}
