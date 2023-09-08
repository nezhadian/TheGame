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
    }
}
