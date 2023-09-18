using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheGame.Server.Data;
using TheGame.Server.Services;
using TheGame.Shared;

namespace TheGame.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AcountController : ControllerBase
    {
        readonly DataContext _context;
        private readonly IUtilityService _utility;

        public AcountController(IUtilityService utility, DataContext context)
        {
            _utility = utility;
            _context = context;
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordData data)
        {
            var user = await _utility.GetUser();
            if (!_utility.VerifyPasswordHash(user.PasswordHash, user.PasswordSalt, data.CurrentPassword))
            {
                return BadRequest(new ServiceResponse<string>
                {
                    IsSuccess = false,
                    Message = "wrong password"
                });
            }
            else
            {
                var password = _utility.GeneratePasswordHash(data.NewPassword);
                user.PasswordHash = password.hash;
                user.PasswordSalt = password.salt;
                await _context.SaveChangesAsync();
                return Ok(new ServiceResponse<string>
                {
                    IsSuccess = true,
                    Data = "your password has been changed"
                });
            }
        }

    }
}
