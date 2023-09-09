using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame.Shared
{
    public class Battle
    {
        public int Id { get; set; }
        
        public int AttackerId { get; set; }
        public User Attacker { get; set; }

        public int OpponentId { get; set; }
        public User Opponent { get; set; }

        public int WinnerId { get; set; }

        public int AttackerDamage { get; set; }
        public int OpponentDamage { get; set; }

        public int AttackerHitpoint { get; set; }
        public int OpponentHitpoint { get; set; }

        public int Rounds { get; set; }

        public bool IsCompleted { get; set; }
        public DateTime BattleDate { get; set; } = DateTime.Now;
    }
}
