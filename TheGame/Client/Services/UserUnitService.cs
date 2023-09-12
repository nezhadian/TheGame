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
    public class UserUnitService : IUserUnitService
    {
        readonly HttpClient _http;
        readonly IToastService _toast;

        public UserUnitService(HttpClient http, IToastService toast)
        {
            _http = http;
            _toast = toast;
        }

        public IList<Unit> Units { get; set; } = new List<Unit>();
        public IList<UserUnit> UserUnits { get; set; } = new List<UserUnit>();

        public async Task GetUnitsAsync()
        {
            Units = await _http.GetFromJsonAsync<IList<Unit>>("api/unit/getall");
        }

        public async Task GetUserUnitsAsync()
        {
            UserUnits = await _http.GetFromJsonAsync<IList<UserUnit>>("api/userunit/getall");
        }

        public async Task ReviveAllUnitsAsync()
        {
            var resault = await _http.PostAsJsonAsync<string>("api/userunit/revive", null);
            if (resault.IsSuccessStatusCode)
            {
                _toast.ShowSuccess(await resault.Content.ReadAsStringAsync());
            }
            else
            {
                _toast.ShowError(await resault.Content.ReadAsStringAsync());
            }

            await GetUserUnitsAsync();
        }
    }
}
