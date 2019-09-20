using UintaPine.CRM.Database;
using UintaPine.CRM.Logic.Server;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using model.UintaPine.Utility;
using System;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using UintaPine.CRM.Model.Server;

namespace UintaPine.CRM.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var settings = Configuration.Get<ApplicationSettings>();
            services.AddSingleton(settings);

            //When an access token is sent to the server, use these rules to validate the token.
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.SigningKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.ExpireTimeSpan = new TimeSpan(1000, 0, 0, 0);
                options.Cookie = new CookieBuilder()
                {
                    Name = "access_token",
                    HttpOnly = true,
                    SameSite = SameSiteMode.None
                };
                options.Events.OnRedirectToLogin = (context) =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
                options.LoginPath = PathString.Empty;
                options.TicketDataFormat = new CustomJwtDataFormat(SecurityAlgorithms.HmacSha256, tokenValidationParameters);
            });

            // Add framework services.
            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("no-cache", new CacheProfile()
                {
                    NoStore = true,
                });
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddSingleton<MongoContext>();
            services.AddSingleton<UtilityLogic>();
            services.AddSingleton<TokenLogic>();
            services.AddSingleton<UserLogic>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder
                    .WithOrigins("http://localhost:50529", "https://uintapine.azurewebsites.net")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseCors("AllowAll");
            app.UseMvcWithDefaultRoute();
        }
    }
}