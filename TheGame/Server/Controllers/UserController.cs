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
    public class UserController : ControllerBase
    {
        readonly DataContext _context;
        private readonly IUtilityService _utility;

        public UserController(IUtilityService utility, DataContext context)
        {
            _utility = utility;
            _context = context;
        }

        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard()
        {
            var users = await _context.Users
                .OrderByDescending(u => u.TotalDamage)
                .ThenByDescending(u => u.Victories)
                .ThenBy(u => u.Defeats)
                .ThenByDescending(u => u.Id)
                .ToListAsync();

            int i = 1;
            var leaderboard = users.Select(u =>
                new UserStatistcs
                {
                    Rank = i++,
                    UserId = u.Id,
                    Username = u.Username,
                    Battles = u.Battles,
                    Victories = u.Victories,
                    Defeats = u.Defeats,
                    TotalDamage = u.TotalDamage
                }
            );

            return Ok(leaderboard);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var user = await _utility.GetUser();
            var battles = await _context.Battles
                .Where(u => u.AttackerId == user.Id || u.OpponentId == user.Id)
                .OrderByDescending(u => u.BattleDate)
                .Include(u => u.Attacker)
                .Include(u => u.Opponent)
                .ToListAsync();

            var history = battles.Select(b => new BattleHistoryEntry
            {
                BattleId = b.Id,
                OpponentName = b.AttackerId == user.Id ? b.Opponent.Username : b.Attacker.Username,
                YouWon = b.WinnerId == user.Id
            });

            return Ok(history);
        }

    }
}
