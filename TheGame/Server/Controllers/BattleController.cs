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

        [HttpGet]
        public async Task<IActionResult> GetInProgressBattleId()
        {
            var battle = await _utility.GetInProgressUserBattle();

            return Ok(battle == null ? -1 : battle.Id);

        }

        [HttpPost("start")]
        public async Task<IActionResult> StartBattle([FromBody] int opponentId)
        {
            var attacker = await _utility.GetUser();
            var opponent = await _context.Users.FindAsync(opponentId);

            if (await _utility.IsUserInBattle())
                return BadRequest($"your last battle isn`t completed");

            if (await _utility.IsInBattle(opponent.Id))
                return BadRequest($"Opponent ({attacker.Username}) is in battle");

            var attackerHitpoint = await CalculateHitpoint(attacker.Id);
            var opponentHitpoint = await CalculateHitpoint(opponent.Id);

            if(attackerHitpoint <= 0)
                return BadRequest($"you are not have alive army");

            if (opponentHitpoint <= 0)
                return BadRequest($"Opponent ({opponent.Username}) is not have army");

            var battle = new Battle
            {
                AttackerId = attacker.Id,
                OpponentId = opponent.Id,
                AttackerHitpoint = attackerHitpoint,
                OpponentHitpoint = opponentHitpoint
            };

            _context.Battles.Add(battle);
            await _context.SaveChangesAsync();

            return Ok(opponent.Username);

        }
        public async Task<int> CalculateHitpoint(int userId)
        {
            var count = await _context.UserUnits
                .Select(u => u.UserId == userId ? u.HitPoint : 0).SumAsync();

            return count;
        }

        [HttpPost("info")]
        public async Task<IActionResult> GetBattleInfo([FromBody] int battleId)
        {
            var user = await _utility.GetUser();
            var battles = await _context.Battles
                .Where(b => b.Id == battleId)
                .Include(b => b.Attacker)
                .Include(b => b.Opponent)
                .ToListAsync();

            

            if(battles.Count <= 0)
                return BadRequest("this battleid isn`t exists");

            var battle = battles[0];

            var battleProgress = new BattleProgress
            {
                BattleId = battle.Id,
                AttackerName = battle.Attacker.Username,
                OpponentName = battle.Opponent.Username,
                AttackerHitpoint = battle.AttackerHitpoint,
                OpponentHitpoint = battle.OpponentHitpoint,
                AttackerDamage = battle.AttackerDamage,
                OpponentDamage = battle.OpponentDamage,
                Rounds = battle.Rounds,
                IsAttackerWinner = battle.IsCompleted ? battle?.WinnerId == battle.AttackerId : null,
                YouWon = battle.IsCompleted ? battle?.WinnerId == user.Id : null,
                IsCompleted = battle.IsCompleted
            };

            return Ok(battleProgress);
        }

    }
}
