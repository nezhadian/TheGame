using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame.Shared
{
    public class AttackResault
    {
        public int Round { get; set; }
        public int AttackerUnitId { get; set; }
        public int OpponentUnitId { get; set; }
        public int Damage { get; set; }
        public bool IsDead { get; set; }
        public bool IsDefence { get; set; }


    }
}
