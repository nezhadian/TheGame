using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TheGame.Server.Data;
using TheGame.Shared;

namespace TheGame.Server.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly DataContext _context;

        public UtilityService(IHttpContextAccessor httpContext, DataContext context)
        {
            _httpContext = httpContext;
            _context = context;
        }

        private int GetUserId()
        {
            return int.Parse(_httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
        }

        public async Task<User> GetUser()
        {
            int userId = GetUserId();
            return await _context.Users.FindAsync(userId);
        }

        

        public Task<bool> IsUserInBattle()
        {
            return IsInBattle(GetUserId());
        }

        public async Task<bool> IsInBattle(int userId)
        {
            return await _context.Battles.AnyAsync(
                u => !u.IsCompleted &&
                (u.AttackerId == userId || u.OpponentId == userId));
        }

        public async Task<Battle> GetInProgressUserBattle()
        {
            var userId = GetUserId();

            var battles = _context.Battles
                .Where(u => u.AttackerId == userId || u.OpponentId == userId)
                .Include(u => u.Attacker)
                .Include(u => u.Opponent);

            return await battles.FirstOrDefaultAsync(b => !b.IsCompleted);
        }
    }
}
