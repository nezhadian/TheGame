using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TheGame.Shared;

namespace TheGame.Client.Services
{
    public class BattleService : IBattleService
    {
        readonly HttpClient _http;
        readonly NavigationManager _navigation;
        readonly IToastService _toast;

        public BattleService(HttpClient http, NavigationManager navigation, IToastService toast)
        {
            _http = http;
            _navigation = navigation;
            _toast = toast;
        }

        public BattleProgress CurrentBattle { get; set; }

        public int CurrentBattleId { get; set; }
        public bool IsUserInBattle { get; set; }

        public async Task GetCurrentBattle()
        {
           var response = await _http.GetFromJsonAsync<int>("api/battle/");

            CurrentBattleId = response;
            IsUserInBattle = response != -1;
        }

        public Task StartBattleAsync(int opponentId)
        {
            throw new NotImplementedException();
        }
    }
}
