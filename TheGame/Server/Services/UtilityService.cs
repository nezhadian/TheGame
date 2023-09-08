using Microsoft.AspNetCore.Http;
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

        public async Task<User> GetUser()
        {
            var userId = int.Parse(_httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
            return await _context.FindAsync<User>(userId);
        }
    }
}
