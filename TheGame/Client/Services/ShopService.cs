using Blazored.Toast.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace TheGame.Client.Services
{
    public class ShopService : IShopService
    {
        private readonly HttpClient _http;
        readonly IToastService _toast;
        readonly IUserUnitService _userUnits;
        public ShopService(HttpClient http, IToastService toast, IUserUnitService userUnits)
        {
            _http = http;
            _toast = toast;
            _userUnits = userUnits;
        }

        public int Costs { get; set; }

        public event Action OnChanged;
        public void RaiseOnChanged()
        {
            OnChanged?.Invoke();
        }

        public async Task GetCostsAsync()
        {
            Costs = await _http.GetFromJsonAsync<int>("api/user/costs");
            RaiseOnChanged();
        }

        public async Task BuyNewItemAsync(int unitId)
        {
            var unit = _userUnits.Units.First(u => u.Id == unitId);
            var response = await _http.PostAsJsonAsync("api/userunit/add", unitId);
            if (response.IsSuccessStatusCode)
            {
                _toast.ShowSuccess($"Successfully added new {unit.Title}");
            }
            else
            {
                _toast.ShowError(await response.Content.ReadAsStringAsync());
            }
            await GetCostsAsync();
            await _userUnits.GetUserUnitsAsync();
        }
    }
}
