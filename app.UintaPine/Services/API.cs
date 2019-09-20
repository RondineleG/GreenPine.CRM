using app.UintaPine.Services;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blazored.Toast;
using Blazored.Toast.Services;
using model.Shared.UintaPine;
using model.Client.UintaPine;
using System.Diagnostics;
using Microsoft.AspNetCore.Blazor.Http;

namespace app.UintaPine.Services
{
    public class API
    {
        [Inject]
        private AppState _appState { get; set; }

        [Inject]
        private NavigationManager _navigationManager { get; set; }
        
        private HttpClient _client;
        
        public API(AppState appState, NavigationManager navigationManager)
        {
            _appState = appState;
            _navigationManager = navigationManager;

            _client = new HttpClient();
            DetectDomainAndSetRootApi(_navigationManager.Uri);
        }

        private void DetectDomainAndSetRootApi(string path)
        {
            var tokenized = path.Split('/');
            string appRoot = tokenized[2];
            if(appRoot == "localhost:50529")
                _client.BaseAddress = new Uri("http://localhost:50119");
            else if(appRoot == "uintapine.azurewebsites.net")
                _client.BaseAddress = new Uri("https://uintapineapi.azurewebsites.net");
        }


        public async Task<UserSlim> RegisterUser(string email, string password, string confirmPassword)
        {
            Register content = new Register()
            {
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            return await Post<UserSlim>("api/v1/user", content);
        }

        public async Task<UserSlim> AuthenticateUser(string username, string password)
        {
            Authenticate content = new Authenticate()
            {
                Email = username,
                Password = password
            };
            
            return await Post<UserSlim>("api/v1/authenticate", content);
        }

        public async Task Logout()
        {
            await Get("api/v1/logout");
        }

        public async Task<UserSlim> GetUserCurrent()
        {
            return await GetAsAsync<UserSlim>("api/v1/user/me");
        }





        #region HttpClient Methods
        private async Task<bool> Get(string path)
        {
            var response = await _client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<T> GetAsAsync<T>(string path)
        {
            var response = await _client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            else
            {
                try
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(responseContent);
                }
                catch
                {
                    return default(T);
                }
            }
        }

        private async Task<T> Post<T>(string path, object content)
        {
            string json = JsonConvert.SerializeObject(content);
            StringContent postContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await _client.PostAsync(path, postContent);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            else
            {
                try
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(responseContent);
                }
                catch
                {
                    return default(T);
                }
            }
        }

        private async Task<T> Put<T>(string path, object content)
        {
            string json = JsonConvert.SerializeObject(content);
            StringContent postContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await _client.PutAsync(path, postContent);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            else
            {
                try
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(responseContent);
                }
                catch
                {
                    return default(T);
                }
            }
        }

        private async Task<T> Delete<T>(string path)
        {
            var response = await _client.DeleteAsync(path);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            else
            {
                try
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(responseContent);
                }
                catch
                {
                    return default(T);
                }
            }
        }
        #endregion
    }
}
