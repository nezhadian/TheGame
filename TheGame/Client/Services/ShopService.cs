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
        public ShopService(HttpClient http)
        {
            _http = http;
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
    }
}
