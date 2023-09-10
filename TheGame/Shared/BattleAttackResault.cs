using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame.Shared
{
    public class BattleAttackResault
    {
        public bool IsCompleted { get; set; }
        public IList<AttackResault> Attacks { get; set; }
    }
}
