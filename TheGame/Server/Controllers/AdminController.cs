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

namespace TheGame.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        readonly IUtilityService _utility;
        readonly DataContext _context;

        public AdminController(IUtilityService utility, DataContext context)
        {
            _utility = utility;
            _context = context;
        }

        [HttpPut("SyncTotalDamage")]
        public async Task<IActionResult> SyncTotalDamage()
        {
            var user = await _utility.GetUser();
            if (user.Id != 1)
                return Unauthorized();

            foreach (var u in _context.Users)
            {
                u.TotalDamage = await CalculateTotalDamage(u.Id);
            }
            await _context.SaveChangesAsync();

            return Ok("Total Damages Synced.");
        }

        private async Task<int> CalculateTotalDamage(int id)
        {
            return await _context.Battles
                .Where(u => u.OpponentId == id || u.AttackerId == id)
                .Where(u => u.IsCompleted)
                .SumAsync(u => u.AttackerId == id ? u.OpponentDamage : u.AttackerDamage);
        }
    }
}
