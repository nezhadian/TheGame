using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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
        readonly ILocalStorageService _storage;
        readonly AuthenticationStateProvider _authState;
        readonly NavigationManager _navigation;


        public AuthService(HttpClient http, ILocalStorageService storage, AuthenticationStateProvider authState, NavigationManager navigation)
        {
            _http = http;
            _storage = storage;
            _authState = authState;
            _navigation = navigation;
        }

        public async Task<ServiceResponse<string>> Login(LoginData data)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", data);
            return await response.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<int>> Register(RegisterData data)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", data);
            return await response.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }

        public async Task Logout()
        {
            await _storage.RemoveItemAsync("token");
            await _authState.GetAuthenticationStateAsync();
            _navigation.NavigateTo("");
        }
    }
}
