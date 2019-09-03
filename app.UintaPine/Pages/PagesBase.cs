using app.UintaPine.Services;
using Microsoft.AspNetCore.Components;
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

        protected override void OnInitialized()
        {
            AppState.Email = "dahln@outlook.com";

            base.OnInitialized();
        }
    }

    public class API
    {
        private HttpClient _client;
        public API()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:53653");
        }
        public async Task Ping()
        {
            var result = await _client.GetAsync("api/v1/ping");
        }
        //public async Task RegisterUser(User user)
        //{
        //    await _client.PostJsonAsync("api/v1/user", user);
        //}
        //public async Task AuthenticateUser(string username, string password)
        //{
        //    Authenticate content = new Authenticate()
        //    {
        //        Email = username,
        //        Password = password
        //    };
        //    await _client.PostJsonAsync("api/v1/authenticate", content);
        //}
    }
}
