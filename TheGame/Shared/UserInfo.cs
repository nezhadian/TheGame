using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame.Shared
{
    public class UserInfo
    {
        public string Email { get; set; }
        public string Username { get; set; }

        public int TotalCosts { get; set; }

        public int Battles { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }

        public int TotalDamage { get; set; }
    }
}
