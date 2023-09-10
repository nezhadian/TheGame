using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame.Shared
{
    public class BattleHistoryEntry
    {
        public int BattleId { get; set; }
        public bool YouWon { get; set; }
        public string OpponentName { get; set; }

    }
}
