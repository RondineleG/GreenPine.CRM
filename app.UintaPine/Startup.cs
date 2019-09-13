using app.UintaPine.Services;
using Blazored.Toast;
using Microsoft.AspNetCore.Blazor.Http;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using model.Client.UintaPine;

namespace app.UintaPine
{
    public class Startup
    {
        

        public void ConfigureServices(IServiceCollection services)
        {
            ApplicationSettings applicationSettings = new ApplicationSettings();
#if DEBUG
            applicationSettings.ApiRoot = "http://localhost:50119";
#endif
#if RELEASE
            applicationSettings.ApiRoot = "http://localhost:50119";
#endif
            services.AddSingleton(applicationSettings);


            services.AddSingleton<AppState>();
            services.AddSingleton<API>();
            services.AddBlazoredToast();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            //https://github.com/aspnet/AspNetCore/issues/9894
            WebAssemblyHttpMessageHandler.DefaultCredentials = FetchCredentialsOption.Include;

            app.AddComponent<App>("app");
        }
    }
}
