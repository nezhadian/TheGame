using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheGame.Server.Services;

namespace TheGame.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUtilityService _utility;

        public UserController(IUtilityService utility)
        {
            _utility = utility;
        }

        [HttpGet("costs")]
        public async Task<IActionResult> GetCosts()
        {
            var user = await _utility.GetUser();
            return Ok(user.TotalCosts);
        }
    }
}
