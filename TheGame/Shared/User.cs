using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame.Shared
{
    public class User
    {
        public int Id { get; set; }
        
        public string Email { get; set; }
        public string Username { get; set; }
        
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public int TotalCosts { get; set; } = 1000;
        
        public int Battles { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }
    }
}
