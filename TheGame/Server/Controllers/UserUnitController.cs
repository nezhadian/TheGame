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

        [HttpPost("add")]
        public async Task<IActionResult> AddUnit([FromBody]int unitId)
        {
            var unit = await _context.Units.FindAsync(unitId);
            if (unit == null)
                return BadRequest("this unit is not exists");

            var user = await _utility.GetUser();

            if(user.TotalCosts < unit.Cost)
            {
                return NotFound("you do not have enough money");
            }

            user.TotalCosts -= unit.Cost;
            var userUnit = new UserUnit
            {
                UnitId = unit.Id,
                UserId = user.Id,
                HitPoint = unit.HitPoint
            };

            _context.UserUnits.Add(userUnit);

            await _context.SaveChangesAsync();

            return Ok(userUnit);
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

    }
}
