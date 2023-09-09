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
    public class BattleController : ControllerBase
    {
        readonly IUtilityService _utility;
        readonly DataContext _context;

        public BattleController(IUtilityService utility, DataContext context)
        {
            _utility = utility;
            _context = context;
        }

        [HttpGet("getmybattle")]
        public async Task<IActionResult> GetUserBattle()
        {
            var user = await _utility.GetUser();

            var battles = _context.Battles
                .Where(u => u.AttackerId == user.Id || u.OpponentId == user.Id)
                .Include(u => u.Attacker)
                .Include(u => u.Opponent);

            var inProgressBattle = await battles.FirstOrDefaultAsync(b => !b.IsCompleted);

            if (inProgressBattle == null)
                return NotFound("you are not in battle");

            var battleProgress = new BattleProgress
            {
                BattleId = inProgressBattle.Id,
                AttackerName = inProgressBattle.Attacker.Username,
                OpponentName = inProgressBattle.Opponent.Username,
                AttackerHitpoint = inProgressBattle.AttackerHitpoint,
                OpponentHitpoint = inProgressBattle.OpponentHitpoint,
                AttackerDamage = inProgressBattle.AttackerDamage,
                OpponentDamage = inProgressBattle.OpponentDamage,
                Rounds = inProgressBattle.Rounds,
            };

            return Ok(battleProgress);
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartBattle([FromBody] int opponentId)
        {
            var attacker = await _utility.GetUser();
            var opponent = await _context.Users.FindAsync(opponentId);

            if (await IsInBattle(attacker.Id))
                return BadRequest($"Attacker ({attacker.Username}) is in battle");

            if (await IsInBattle(opponent.Id))
                return BadRequest($"Opponent ({attacker.Username}) is in battle");

            var attackerHitpoint = await CalculateHitpoint(attacker.Id);
            var opponentHitpoint = await CalculateHitpoint(opponent.Id);

            if(attackerHitpoint == 0)
                return BadRequest($"Attacker ({attacker.Username}) is not have army");

            if (opponentHitpoint == 0)
                return BadRequest($"Opponent ({opponent.Username}) is not have army");

            var battle = new Battle
            {
                AttackerId = attacker.Id,
                OpponentId = opponentId,
                AttackerHitpoint = attackerHitpoint,
                OpponentHitpoint = opponentHitpoint
            };

            _context.Battles.Add(battle);
            await _context.SaveChangesAsync();

            var battleProgress = new BattleProgress
            {
                BattleId = battle.Id,
                AttackerName = attacker.Username,
                OpponentName = opponent.Username,
                AttackerHitpoint = attackerHitpoint,
                OpponentHitpoint = opponentHitpoint
            };

            return Ok(battleProgress);

        }

        public async Task<int> CalculateHitpoint(int userId)
        {
            var count = await _context.UserUnits
                .Select(u => u.UserId == userId ? u.HitPoint :0).SumAsync();

            return count;
        }
        public async Task<bool> IsInBattle(int userId)
        {
            return await _context.Battles.AnyAsync(
                u => !u.IsCompleted &&  
                (u.AttackerId == userId || u.OpponentId == userId));
        }




    }
}
