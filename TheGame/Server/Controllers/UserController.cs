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
    public class UserController : ControllerBase
    {
        readonly DataContext _context;
        private readonly IUtilityService _utility;

        public UserController(IUtilityService utility, DataContext context)
        {
            _utility = utility;
            _context = context;
        }

        [HttpGet("costs")]
        public async Task<IActionResult> GetCosts()
        {
            var user = await _utility.GetUser();
            return Ok(user.TotalCosts);
        }

        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard()
        {
            var users = await _context.Users
                .OrderByDescending(u => u.Victories)
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
                    Defeats = u.Defeats
                }
            );

            return Ok(leaderboard);
        }
    }
}
