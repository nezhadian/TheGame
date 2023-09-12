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

    public class ShopController : ControllerBase
    {
        readonly DataContext _context;
        private readonly IUtilityService _utility;

        public ShopController(DataContext context, IUtilityService utility)
        {
            _context = context;
            _utility = utility;
        }


        [HttpGet("costs")]
        public async Task<IActionResult> GetCosts()
        {
            var user = await _utility.GetUser();
            return Ok(user.TotalCosts);
        }

        [HttpPost("buy")]
        public async Task<IActionResult> BuyNewItem([FromBody] int unitId)
        {
            var unit = await _context.Units.FindAsync(unitId);
            if (unit == null)
                return BadRequest("this unit is not exists");

            var user = await _utility.GetUser();

            if (user.TotalCosts < unit.Cost)
            {
                return NotFound("you do not have enough money");
            }
            if (await _utility.IsUserInBattle())
            {
                return BadRequest("during a battle you can`t buy new items");
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
    }
}
