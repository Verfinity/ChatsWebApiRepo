using ChatsFrontend.Components;
using ChatsFrontend.Logic.HttpClients;
using ChatsFrontend.Logic.SavingData.TokensSaver;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net;

namespace ChatsFrontend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            string baseAddress = builder.Configuration["ApiAddress"];
            string authScheme = builder.Configuration["AuthorizationScheme"];
            builder.Services.AddScoped<IAuthHttpClient>(serviceProvider =>
            {
                var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(baseAddress)
                };
                var tokenPairSaver = serviceProvider.GetRequiredService<ITokenPairSaver>();

                return new AuthHttpClient(httpClient, tokenPairSaver, authScheme);
            });

            string tokenPairSaveKey = builder.Configuration.GetSection("StorageKeys")["SaveTokensKey"];
            builder.Services.AddScoped<ITokenPairSaver>(serviceProvider =>
            {
                return new TokenPairSaver(tokenPairSaveKey, serviceProvider.GetRequiredService<ProtectedLocalStorage>());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
