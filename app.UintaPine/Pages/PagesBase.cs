using app.UintaPine.Services;
using Microsoft.AspNetCore.Components;
using model.UintaPine.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace app.UintaPine.Pages
{
    public class PagesBase : ComponentBase
    {
        [Inject]
        protected AppState AppState { get; set; }
        
        protected API _api { get; set; } = new API();
    }

    public class API
    {
        private HttpClient _client;
        public API()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:50119");
        }
        public async Task Ping()
        {
            var result = await _client.GetAsync("api/v1/ping");
        }
        
        public async Task<UserSlim> RegisterUser(Register content)
        {
            var response = await _client.PostJsonAsync<UserSlim>("api/v1/user", content);
            return response;
        }

        public async Task<UserSlim> AuthenticateUser(string username, string password)
        {
            Authenticate content = new Authenticate()
            {
                Email = username,
                Password = password
            };
            var response = await _client.PostJsonAsync<UserSlim>("api/v1/authenticate", content);
            return response;
        }

        public async Task Logout()
        {
            await _client.GetAsync("api/v1/logout");
        }

        public async Task<UserSlim> GetUserCurrent()
        {
            var response = await _client.GetJsonAsync<UserSlim>("api/v1/user/me");
            return response;
        }
    }
}
