using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TheGame.Client.Services;
using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using TheGame.Client.Classes;

namespace TheGame.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            
            //me
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IShopService, ShopService>();
            builder.Services.AddScoped<IUserUnitService, UserUnitService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IBattleService, BattleService>();
            builder.Services.AddScoped<IAttackService, AttackService>();
            builder.Services.AddScoped<IBattleInfoService, BattleInfoService>();

            //blazored
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazoredToast();

            //authorization
            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();


            await builder.Build().RunAsync();
        }
    }
}
