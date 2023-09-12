using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    public class UserUnitController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUtilityService _utility;

        public UserUnitController(IUtilityService utility, DataContext context)
        {
            _utility = utility;
            _context = context;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetUserUnits()
        {
            var user = await _utility.GetUser();
            var userUnits = await _context.UserUnits
                .Where(u => u.UserId == user.Id)
                .Select(u => new UserUnitResponse
                {
                    UnitId = u.UnitId,
                    HitPoint = u.HitPoint
                })
                .ToListAsync();

            return Ok(userUnits);
        }

        [HttpPost("revive")]
        public async Task<IActionResult> ReviveAllUnits()
        {
            var user = await _utility.GetUser();

            var reviveDollars = 1000;

            if (user.TotalCosts < reviveDollars)
            {
                return BadRequest("you do not have enough money for revive(1000$)");
            }

            var units = await _context.UserUnits
                .Where(u => u.UserId == user.Id)
                .Include(u => u.Unit)
                .ToListAsync();

            Random rnd = new Random();
            bool isAnyRevived = false;
            foreach (var unit in units)
            {
                //is dead
                if (unit.HitPoint <= 0)
                {
                    isAnyRevived = true;
                    unit.HitPoint = rnd.Next(unit.Unit.HitPoint);
                }
            }

            if (!isAnyRevived)
                return Ok("all your armies still alive ");


            user.TotalCosts -= reviveDollars;
            await _context.SaveChangesAsync();
            return Ok("you armies revived");
        }

    }
}
