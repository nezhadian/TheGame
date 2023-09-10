using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheGame.Shared;

namespace TheGame.Client.Services
{
    public interface IUserService
    {
        public IList<UserStatistcs> Leaderboard { get; set; }
        public IList<BattleHistoryEntry> History { get; set; }

        Task GetLeaderboardAsync();
        Task GetHistoryAsync();
    }
}
