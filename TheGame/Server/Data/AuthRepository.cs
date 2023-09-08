using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TheGame.Shared;

namespace TheGame.Server.Data
{
    public class AuthRepository : IAuthRepository
    {
        readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<AuthResponse<string>> Login(string usernameOrEmail, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResponse<int>> Register(User user, string password)
        {
            if (IsUserExists(user.Email,user.Username))
            {
                return new AuthResponse<int>
                {
                    IsSuccess = false,
                    Message = "this email or username is used by another user"
                };
            }

            var passwordHash = GenerateHash(password);
            user.PasswordHash = passwordHash.hash;
            user.PasswordSalt = passwordHash.salt;

            await _context.Users.AddAsync(user);
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
        public bool IsUserExists(string email, string username)
        {
            string lowerEmail = email.ToLower();
            string lowerUsername = email.ToLower();

            var user = _context.Users.FirstOrDefault(
                u => u.Email.ToLower() == lowerEmail ||
                     u.Username == username);

            return user != null;
        }

    }
}
