﻿using Microsoft.AspNetCore.Authorization;
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
    public class AttackController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUtilityService _utility;

        public AttackController(DataContext context, IUtilityService utility)
        {
            _context = context;
            _utility = utility;
        }

        [HttpGet()]
        public async Task<IActionResult> Attack()
        {
            //find the battle
            var user = await _utility.GetUser();
            var battle = await _utility.GetInProgressUserBattle();

            if (battle == null)
                return BadRequest("you are not in battle");


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

            battle.Rounds = round;
            await _context.Attacks.AddRangeAsync(attacks);
            await _context.SaveChangesAsync();

            return Ok(new BattleAttackResault
            {
                IsCompleted = battle.IsCompleted,
                Attacks = AttackToAttackResault(user, attacks).ToList()
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

            battle.IsCompleted = true;        }
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

            if (damage > randomOpponent.HitPoint)
            {
                //unit death
                damage = randomOpponent.HitPoint;
                randomOpponent.HitPoint -= damage;
                opponentArmy.Remove(randomOpponent);

                attacks.Add(new Attack
                {
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
        private static IEnumerable<AttackResault> AttackToAttackResault(User user, List<Attack> attacks)
        {
            return attacks.Select(a => new AttackResault
            {
                Round = a.Round,
                AttackerUnitId = a.AttackerUnit.UnitId,
                OpponentUnitId = a.OpponentUnit.UnitId,
                Damage = a.Damage,
                IsDead = a.IsUnitDead,
                IsDefence = a.OpponentUnit.UserId == user.Id
            });
        }
    }
}
