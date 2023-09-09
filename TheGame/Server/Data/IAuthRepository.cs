using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheGame.Shared;

namespace TheGame.Server.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<string>> Login(string usernameOrEmail,string password);
        Task<ServiceResponse<int>> Register(User user,string password);
        Task<bool> IsUserExists(string email,string username);
    }
}
