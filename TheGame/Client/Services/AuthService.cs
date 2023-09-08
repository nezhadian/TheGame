using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TheGame.Shared;

namespace TheGame.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;

        public AuthService(HttpClient http)
        {
            _http = http;
        }

        public async Task<AuthResponse<string>> Login(LoginData data)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", data);
            return await response.Content.ReadFromJsonAsync<AuthResponse<string>>();
        }

        public async Task<AuthResponse<int>> Register(RegisterData data)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", data);
            return await response.Content.ReadFromJsonAsync<AuthResponse<int>>();
        }
    }
}
