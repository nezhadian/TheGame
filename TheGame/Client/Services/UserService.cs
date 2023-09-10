using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TheGame.Shared;

namespace TheGame.Client.Services
{
    public class UserService : IUserService
    {
        public IList<UserStatistcs> Leaderboard { get; set; } = new List<UserStatistcs>();
        public IList<BattleHistoryEntry> History { get; set; } = new List<BattleHistoryEntry>();

        readonly HttpClient _http;

        public UserService(HttpClient http)
        {
            _http = http;
        }

        public async Task GetLeaderboardAsync()
        {
            var response = await _http.GetFromJsonAsync<IList<UserStatistcs>>("api/user/leaderboard");
            Leaderboard = response;
        }

        public async Task GetHistoryAsync()
        {
            var response = await _http.GetFromJsonAsync<IList<BattleHistoryEntry>>("api/user/history");
            History = response;
        }
    }
}
