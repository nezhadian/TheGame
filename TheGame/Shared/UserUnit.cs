using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame.Shared
{
    public class UserUnit
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }
        
        public int UnitId { get; set; }
        public Unit Unit { get; set; }

        public int HitPoint { get; set; }
    }
}
