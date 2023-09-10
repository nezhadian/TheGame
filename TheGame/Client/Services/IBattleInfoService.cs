using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheGame.Shared;

namespace TheGame.Client.Services
{
    public interface IBattleInfoService
    {
        public BattleProgress CurrentBattle { get; set; }
        public event Action OnChanged;

        Task GetBattleInfo(int battleId);
        Task ReloadCurrentBattle();
    }
}
