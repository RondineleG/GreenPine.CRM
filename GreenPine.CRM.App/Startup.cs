using Blazored.LocalStorage;
using Blazored.Toast;
using GreenPine.CRM.App.Services;
using Microsoft.AspNetCore.Blazor.Http;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace GreenPine.CRM.App
{
    public class Startup
    {
        

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<AppState>();
            services.AddScoped<API>();

            services.AddBlazoredLocalStorage();
            services.AddBlazoredToast();
            services.AddLoadingBar();

        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.UseLoadingBar();

            //https://github.com/aspnet/AspNetCore/issues/9894
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("WEBASSEMBLY")))
            {
                WebAssemblyHttpMessageHandler.DefaultCredentials = FetchCredentialsOption.Include;
            }

            app.AddComponent<App>("app");
        }
    }
}
