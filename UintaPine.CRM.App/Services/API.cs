using UintaPine.CRM.App.Services;
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
using Blazored.Toast.Configuration;
using UintaPine.CRM.Model.Shared;
using System.Diagnostics;
using Microsoft.AspNetCore.Blazor.Http;
using UintaPine.CRM.Model.Shared.Responses;
using UintaPine.CRM.Model.Shared.Requests;

namespace UintaPine.CRM.App.Services
{
    public class API
    {
        private AppState _appState { get; set; }

        private NavigationManager _navigationManager { get; set; }

        private IToastService _toastService { get; set; }
        
        private HttpClient _client;
        
        public API(AppState appState, NavigationManager navigationManager, IToastService toastService)
        {
            _appState = appState;
            _navigationManager = navigationManager;
            _toastService = toastService;

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


        public async Task<User> RegisterUser(string email, string password, string confirmPassword)
        {
            Register content = new Register()
            {
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            return await Post<User>("api/v1/user", content);
        }

        public async Task<User> AuthenticateUser(string username, string password)
        {
            Authenticate content = new Authenticate()
            {
                Email = username,
                Password = password
            };
            
            return await Post<User>("api/v1/authenticate", content);
        }

        public async Task Logout()
        {
            await Get("api/v1/logout");
        }

        public async Task<User> GetUserCurrent()
        {
            return await GetAsAsync<User>("api/v1/user/me");
        }


        public async Task<Company> CreateCompany(CreateCompany content)
        {
            return await Post<Company>("api/v1/company", content);
        }

        public async Task<Company> GetCompanyById(string companyId)
        {
            return await GetAsAsync<Company>($"api/v1/company/{companyId}");
        }

        public async Task<List<Company>> GetCompaniesByUser(string userId)
        {
            return await GetAsAsync<List<Company>>($"api/v1/company/user/{userId}");
        }

        public async Task<CustomerTag> CreateTagByCompanyId(string companyId, string name, string backgroundColor, string fontColor)
        {
            CreateTag content = new CreateTag()
            {
                Name = name,
                BackgroundColor = backgroundColor,
                FontColor = fontColor
            };
            await Post($"api/v1/company/{companyId}/tag", content);

            CustomerTag tag = new CustomerTag()
            {
                Name = content.Name,
                BackgroundColor = content.BackgroundColor,
                FontColor = content.FontColor
            };

            return tag;
        }

        public async Task DeleteTagByCompanyIdTagId(string companyId, string tagId)
        {
            await Delete($"api/v1/company/{companyId}/tag/{tagId}");
        }


#region HttpClient Methods
        private async Task<bool> Get(string path)
        {
            var httpWebRequest = new HttpRequestMessage(HttpMethod.Get, path);
            httpWebRequest.Properties[WebAssemblyHttpMessageHandler.FetchArgs] = new
            {
                credentials = "include"
            };
            var response = await _client.SendAsync(httpWebRequest);

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
            var httpWebRequest = new HttpRequestMessage(HttpMethod.Get, path);
            httpWebRequest.Properties[WebAssemblyHttpMessageHandler.FetchArgs] = new
            {
                credentials = "include"
            };
            var response = await _client.SendAsync(httpWebRequest);
            
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            else
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(responseContent) == false)
                    _appState.GlobalToast = responseContent;
                return default(T);
            }
        }

        private async Task Post(string path, object content)
        {
            string json = JsonConvert.SerializeObject(content);
            StringContent postContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var httpWebRequest = new HttpRequestMessage(HttpMethod.Post, path);
            httpWebRequest.Content = postContent;
            httpWebRequest.Properties[WebAssemblyHttpMessageHandler.FetchArgs] = new
            {
                credentials = "include"
            };
            var response = await _client.SendAsync(httpWebRequest);

            if (response.IsSuccessStatusCode)
            {
                //Do nothing
            }
            else
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(responseContent) == false)
                    _appState.GlobalToast = responseContent;
            }
        }

        private async Task<T> Post<T>(string path, object content)
        {
            string json = JsonConvert.SerializeObject(content);
            StringContent postContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var httpWebRequest = new HttpRequestMessage(HttpMethod.Post, path);
            httpWebRequest.Content = postContent;
            httpWebRequest.Properties[WebAssemblyHttpMessageHandler.FetchArgs] = new
            {
                credentials = "include"
            };
            var response = await _client.SendAsync(httpWebRequest);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            else
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if(string.IsNullOrEmpty(responseContent) == false)
                    _appState.GlobalToast = responseContent;
                return default(T);
            }
        }

        
        private async Task<T> Put<T>(string path, object content)
        {
            string json = JsonConvert.SerializeObject(content);
            StringContent postContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var httpWebRequest = new HttpRequestMessage(HttpMethod.Put, path);
            httpWebRequest.Content = postContent;
            httpWebRequest.Properties[WebAssemblyHttpMessageHandler.FetchArgs] = new
            {
                credentials = "include"
            };
            var response = await _client.SendAsync(httpWebRequest);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            else
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(responseContent) == false)
                    _appState.GlobalToast = responseContent;
                return default(T);
            }
        }

        private async Task Delete(string path)
        {
            var httpWebRequest = new HttpRequestMessage(HttpMethod.Delete, path);
            httpWebRequest.Properties[WebAssemblyHttpMessageHandler.FetchArgs] = new
            {
                credentials = "include"
            };
            var response = await _client.SendAsync(httpWebRequest);

            if (response.IsSuccessStatusCode)
            {
                //Do Nothing
            }
            else
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(responseContent) == false)
                    _appState.GlobalToast = responseContent;
            }
        }
#endregion
    }
}
