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
        public bool KeepAttacks { get; set; }
        public event Action OnChanged;

        public async Task<bool> Attack()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<BattleAttackResault>>("api/attack");

            if (response.IsSuccess)
            {
                if (KeepAttacks)
                {
                    Attacks = response.Data.Attacks
                    .Concat(Attacks)
                    .OrderByDescending(u => u.Round)
                    .ToList();
                }
                else
                {
                    Attacks = response.Data.Attacks
                        .OrderByDescending(u => u.Round)
                        .ToList(); ;
                }
                

                RaiseOnChanged();
                return response.Data.IsCompleted;
            }
            else
            {
                _toast.ShowInfo(response.Message);
            }
            return false;
        }

        public async Task GetLog(int battleId)
        {
            var response = await _http.GetFromJsonAsync<IList<AttackResault>>
                ($"api/attack/log/{battleId}");


            Attacks = response;
            RaiseOnChanged();
        }

        public async Task GetMoreLog(int battleId)
        {
            var response = await _http.GetFromJsonAsync<IList<AttackResault>>
                ($"api/attack/more/{battleId}/{Attacks.Count}");

            
            Attacks = Attacks.Concat(response).ToList();
            RaiseOnChanged();
        }

        private void RaiseOnChanged()
        {
            OnChanged?.Invoke();
        }
    }
}
