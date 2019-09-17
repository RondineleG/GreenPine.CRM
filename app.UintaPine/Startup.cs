using app.UintaPine.Services;
using Blazored.Toast;
using Microsoft.AspNetCore.Blazor.Http;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace app.UintaPine
{
    public class Startup
    {
        

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<AppState>();
            services.AddSingleton<API>();
            services.AddBlazoredToast();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            //https://github.com/aspnet/AspNetCore/issues/9894
            //WebAssemblyHttpMessageHandler.DefaultCredentials = FetchCredentialsOption.Include;
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("WEBASSEMBLY")))
            {
                WebAssemblyHttpMessageHandler.DefaultCredentials = FetchCredentialsOption.Include;
            }

            app.AddComponent<App>("app");
        }
    }
}
