using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheGame.Server.Data;
using TheGame.Shared;

namespace TheGame.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IAuthRepository _auth;

        public AuthController(IAuthRepository auth)
        {
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterData data)
        {
            var response = await _auth.Register(new User
            {
                Email = data.Email,
                Username = data.Username,
            }, data.Password);

            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response);
        }

        

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginData data)
        {
            var response = await _auth.Login(data.Email, data.Password);


            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response);
        }
    }
}
