﻿using Blazored.Toast.Services;
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

        public BattleInfoService(HttpClient http)
        {
            _http = http;
        }

        public BattleProgress CurrentBattle { get; set; } = null;

        public event Action OnChanged;

        public async Task GetBattleInfo(int battleId)
        {
            var response = await _http.PostAsJsonAsync("api/battle/info",battleId);
            if (response.IsSuccessStatusCode)
            {
                CurrentBattle = await response.Content.ReadFromJsonAsync<BattleProgress>();
                OnChanged?.Invoke();
            }
            else
            {
                CurrentBattle = null;
            }
            
        }

        public async Task ReloadCurrentBattle()
        {
            await GetBattleInfo(CurrentBattle.BattleId);
        }
    }
}
