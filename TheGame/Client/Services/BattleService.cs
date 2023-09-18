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
        readonly IToastService _toast;
        readonly NavigationManager _navigation;

        public BattleService(HttpClient http, NavigationManager navigation, IToastService toast)
        {
            _http = http;
            _navigation = navigation;
            _toast = toast;
        }

        public BattleProgress CurrentBattle { get; set; }
        public event Action OnChanged;


        public int CurrentBattleId { get; set; }
        public bool IsUserInBattle { get; set; }

        public async Task GetCurrentBattleId()
        {
           var response = await _http.GetFromJsonAsync<int>("api/battle/");

            CurrentBattleId = response;
            IsUserInBattle = response != -1;
            RaiseBattleChanged();
        }

        public async Task StartBattleAsync(int opponentId)
        {
            var response = await _http.PostAsJsonAsync("api/battle/start", opponentId);
            if (response.IsSuccessStatusCode)
            {
                _toast.ShowSuccess($"Battle Started with {await response.Content.ReadAsStringAsync()}");
                _navigation.NavigateTo("/history");
            }
            else
            {
                _toast.ShowInfo(await response.Content.ReadAsStringAsync());
            }
            await GetCurrentBattleId();

        }

        public void RaiseBattleChanged()
        {
            OnChanged?.Invoke();
        }
    }
}
