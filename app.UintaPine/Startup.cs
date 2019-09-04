using app.UintaPine.Services;
using Microsoft.AspNetCore.Blazor.Http;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;

namespace app.UintaPine
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<AppState>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            //https://github.com/aspnet/AspNetCore/issues/9894
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("WEBASSEMBLY")))
            {
                WebAssemblyHttpMessageHandler.DefaultCredentials = FetchCredentialsOption.Include;
            }

            app.AddComponent<App>("app");
        }
    }
}
