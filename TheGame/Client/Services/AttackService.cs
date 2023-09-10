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
    public class AttackService : IAttackService
    {
        private readonly IBattleService _battle;
        private readonly HttpClient _http;
        private readonly IToastService _toast;

        public AttackService(HttpClient http, IBattleService battle)
        {
            _http = http;
            _battle = battle;
        }

        public IList<AttackResault> Attacks { get; set; } = new List<AttackResault>();

        public async Task Fight()
        {            
            throw new NotImplementedException();
        }

        public async Task GetLog(int battleId)
        {
            var response = await _http.PostAsJsonAsync("api/attack/log",battleId);

            if (response.IsSuccessStatusCode)
            {
                Attacks = await response.Content.ReadFromJsonAsync<IList<AttackResault>>();
            }
            else
            {
                _toast.ShowInfo(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
