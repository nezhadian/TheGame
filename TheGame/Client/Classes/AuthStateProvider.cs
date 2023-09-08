using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace TheGame.Client.Classes
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _storage;
        private readonly HttpClient _http;

        public AuthStateProvider(ILocalStorageService storage, HttpClient http)
        {
            _storage = storage;
            _http = http;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _storage.GetItemAsStringAsync("token");
            var identity = new ClaimsIdentity();
            _http.DefaultRequestHeaders.Authorization = null ;

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    identity = new ClaimsIdentity(ParseClaimsFromJWT(token), "jwt");
                    _http.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer",token.Replace("\"",""));
                }
                catch
                {
                    identity = new ClaimsIdentity();
                    _http.DefaultRequestHeaders.Authorization = null;
                    await _storage.RemoveItemAsync("token");
                }
            }

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }

        private IEnumerable<Claim> ParseClaimsFromJWT(string token)
        {
            var payload = token.Split(".")[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var json = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            var claims = json.Select(i => new Claim(i.Key, i.Value.ToString()));

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
