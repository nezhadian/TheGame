using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheGame.Shared;

namespace TheGame.Client.Services
{
    public interface IBattleService
    {
        public BattleProgress CurrentBattle { get; set; }

        public int CurrentBattleId { get; set; }
        public bool IsUserInBattle { get; set; }

        Task GetCurrentBattle();
        Task StartBattleAsync(int opponentId);

    }
}

