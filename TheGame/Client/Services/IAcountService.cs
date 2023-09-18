using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheGame.Shared;

namespace TheGame.Client.Services
{
    public interface IAcountService
    {
        Task<ServiceResponse<string>> ChangePassword(ChangePasswordData data);
    }
}
