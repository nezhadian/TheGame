using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TheGame.Shared;

namespace TheGame.Server.Data
{
    public class AuthRepository : IAuthRepository
    {
        readonly DataContext _context;
        readonly IConfiguration _configuration;

        public AuthRepository(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        //
        public async Task<AuthResponse<string>> Login(string usernameOrEmail, string password)
        {
            string lowerUsernameOrEmail = usernameOrEmail.ToLower();
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                 u.Email.ToLower() == lowerUsernameOrEmail ||
                 u.Username.ToLower() == lowerUsernameOrEmail);
                
            if(user == null)
            {
                return new AuthResponse<string>
                {
                    IsSuccess = false,
                    Message = "this user is not exists"
                };

            }else if (!IsCorrectPassword(user.PasswordHash, user.PasswordSalt, password))
            {
                return new AuthResponse<string>
                {
                    IsSuccess = false,
                    Message = "wrong password"
                };
            }
            else
            {
                return new AuthResponse<string>
                {
                    IsSuccess = true,
                    Data = GenerateToken(user)
                };
            }
        }

        private string GenerateToken(User user)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            };

            var byteKey = Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value.ToString());
            var key = new SymmetricSecurityKey(byteKey);
            
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
            
            var token = new JwtSecurityToken(
                claims : claims,
                expires : DateTime.Now.AddDays(2),
                signingCredentials:creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
                
        }

        private bool IsCorrectPassword(byte[] passwordHash, byte[] passwordSalt, string password)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < hash.Length; i++)
                {
                    if (hash[i] != passwordHash[i])
                        return false;
                }
                return true;
            }
        }

        //
        public async Task<AuthResponse<int>> Register(User user, string password)
        {
            var passwordHash = GenerateHash(password);
            user.PasswordHash = passwordHash.hash;
            user.PasswordSalt = passwordHash.salt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new AuthResponse<int>
            {
                Data = user.Id,
                IsSuccess = true,
            };
        }
        public (byte[] hash,byte[] salt) GenerateHash(string password)
        {
            using(var hmac = new HMACSHA512())
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return (hash, hmac.Key);
            }
        }

        //
        public async Task<bool> IsUserExists(string email, string username)
        {
            string lowerEmail = email.ToLower();
            string lowerUsername = email.ToLower();

            var user = await _context.Users.FirstOrDefaultAsync(
                u => u.Email.ToLower() == lowerEmail ||
                     u.Username == username);

            return user != null;
        }
    }
}
