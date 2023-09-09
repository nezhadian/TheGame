using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame.Shared
{
    public class BattleProgress
    {
        public int BattleId { get; set; }
        public string AttackerName { get; set; }
        public string OpponentName { get; set; }
        
        public int AttackerDamage { get; set; }
        public int AttackerHitpoint { get; set; }

        public int OpponentDamage { get; set; }
        public int OpponentHitpoint { get; set; }

        public int Rounds { get; set; }


    }
}
