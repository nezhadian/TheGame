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
    public class AcountService : IAcountService
    {
        readonly HttpClient _http;
        readonly IToastService _toast;

        public AcountService(HttpClient http, IToastService toast)
        {
            _http = http;
            _toast = toast;
        }

        public async Task<ServiceResponse<string>> ChangePassword(ChangePasswordData data)
        {
            var response = await _http.PostAsJsonAsync("api/acount/changePassword",data);

            return await response.Content.ReadFromJsonAsync<ServiceResponse<string>>();

        }
    }
}
