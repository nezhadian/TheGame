using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheGame.Shared;

namespace TheGame.Server.Services
{
    public interface IUtilityService
    {
        Task<User> GetUser();

        Task<Battle> GetInProgressUserBattle();

        Task<bool> IsUserInBattle();

        Task<bool> IsInBattle(int userId);

        bool VerifyPasswordHash(byte[] passwordHash, byte[] passwordSalt, string password);
        (byte[] hash, byte[] salt) GeneratePasswordHash(string password);


    }
}
