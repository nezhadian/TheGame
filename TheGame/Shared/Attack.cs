using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame.Shared
{
    public class Attack
    {
        public int Id { get; set; }

        public int Round { get; set; }

        public int BattleId { get; set; }
        public Battle Battle { get; set; }

        public int AttackerUnitId { get; set; }
        public UserUnit AttackerUnit { get; set; }

        public int OpponentUnitId { get; set; }
        public UserUnit OpponentUnit { get; set; }

        public int Damage { get; set; }

        public bool IsUnitDead { get; set; }
    }
}
