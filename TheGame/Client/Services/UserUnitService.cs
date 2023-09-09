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

        public UserUnitService(HttpClient http)
        {
            _http = http;
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
    }
}
