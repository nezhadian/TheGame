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
                return Ok(new ServiceResponse<BattleProgress> {
                    IsSuccess = false,
                    Message = "you are not in battle"
                });


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

            return Ok(new ServiceResponse<BattleProgress> {
                IsSuccess = true,
                Data = battleProgress
            });
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

        
        [HttpPost("attack")]
        public async Task<IActionResult> AttackInBattle([FromBody] int battleId)
        {
            //find the battle
            var battle = await _context.Battles.FindAsync(battleId);

            if (battle.IsCompleted)
                return BadRequest("this battle is end");

            //declare vars
            var attacks = new List<Attack>();

            var attackerArmy = await GetArmy(battle.AttackerId);
            var opponentArmy = await GetArmy(battle.OpponentId);
            int round = battle.Rounds;

            //fight
            for (int i = 0; i < 10 && attackerArmy.Count > 0 && opponentArmy.Count > 0; i++)
            {
                round++;
                if (round % 2 == 0)
                {
                    battle.OpponentDamage += FightRound(attackerArmy, opponentArmy, battle, round, attacks);
                }
                else
                {
                    battle.AttackerDamage += FightRound(opponentArmy, attackerArmy, battle, round, attacks);

                }
            }

            //finish
            if (attackerArmy.Count == 0 || opponentArmy.Count == 0)
            {
                FinishBattle(battle, opponentArmy.Count == 0, round);
            }

            await _context.Attacks.AddRangeAsync(attacks);
            await _context.SaveChangesAsync();

            return Ok(new BattleAttackResault {
                IsCompleted = battle.IsCompleted,
                Attacks = attacks
            });
        }

        private static void FinishBattle(Battle battle, bool isAttackerWinner, int round)
        {

            if (isAttackerWinner)
            {
                battle.WinnerId = battle.AttackerId;
                battle.Attacker.TotalCosts += battle.OpponentDamage;
                battle.Opponent.TotalCosts += battle.AttackerDamage * 10;
            }
            else
            {
                battle.WinnerId = battle.OpponentId;
                battle.Opponent.TotalCosts += battle.AttackerDamage;
                battle.Attacker.TotalCosts += battle.OpponentDamage * 10;
            }

            battle.IsCompleted = true;
            battle.Rounds = round;
        }

        private int FightRound(IList<UserUnit> attackerArmy, IList<UserUnit> opponentArmy, Battle battle, int round, List<Attack> attacks)
        {
            var rnd = new Random();

            //select units
            var randomAttacker = attackerArmy[rnd.Next(attackerArmy.Count)];
            var randomOpponent = opponentArmy[rnd.Next(opponentArmy.Count)];

            //damge
            var damage = rnd.Next(randomAttacker.Unit.Attack) - rnd.Next(randomOpponent.Unit.Defence);

            damage = damage < 0 ? 0 : damage;
            //log

            if(damage > randomOpponent.HitPoint)
            {
                //unit death
                damage = randomOpponent.HitPoint;
                randomOpponent.HitPoint -= damage;
                opponentArmy.Remove(randomOpponent);

                attacks.Add(new Attack {
                    BattleId = battle.Id,
                    AttackerUnitId = randomAttacker.Id,
                    OpponentUnitId = randomOpponent.Id,
                    Damage = damage,
                    Round = round,
                    IsUnitDead = true
                });
            }
            else
            {
                //unit damaged
                randomAttacker.HitPoint -= damage;
                attacks.Add(new Attack
                {
                    BattleId = battle.Id,
                    AttackerUnitId = randomAttacker.Id,
                    OpponentUnitId = randomOpponent.Id,
                    Damage = damage,
                    Round = round
                });
            }

            return damage;


        }

        private async Task<IList<UserUnit>> GetArmy(int userId)
        {
            var userUnits = await _context.UserUnits
                .Where(u => u.UserId == userId && u.HitPoint > 0)
                .Include(u => u.Unit)
                .ToListAsync();

            return userUnits;
        }
    }
}
