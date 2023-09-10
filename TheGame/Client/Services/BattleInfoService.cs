using Blazored.Toast.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TheGame.Shared;

namespace TheGame.Client.Services
{
    public class BattleInfoService : IBattleInfoService
    {
        readonly HttpClient _http;
        readonly IAttackService _attack;
        readonly IToastService _toast;

        public BattleInfoService(HttpClient http, IToastService toast)
        {
            _http = http;
            _toast = toast;
        }

        public BattleProgress CurrentBattle { get; set; } = new BattleProgress();
        public IList<AttackResault> Attacks { get; set; } = new List<AttackResault>();

        public async Task GetBattleInfo(int battleId)
        {
            var response = await _http.PostAsJsonAsync("api/battle/info",battleId);
            if (response.IsSuccessStatusCode)
            {
                CurrentBattle = await response.Content.ReadFromJsonAsync<BattleProgress>();
                await _attack.GetLog(CurrentBattle.BattleId);
                Attacks = _attack.Attacks;
            }
            else
            {
                _toast.ShowError(await response.Content.ReadAsStringAsync());
            }
            
        }

        public async Task ReloadCurrentBattle()
        {
            await GetBattleInfo(CurrentBattle.BattleId);
        }
    }
}
