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

            return await PostAsync<User>("api/v1/user", content);
        }

        public async Task<User> AuthenticateUser(string username, string password)
        {
            Authenticate content = new Authenticate()
            {
                Email = username,
                Password = password
            };
            
            return await PostAsync<User>("api/v1/authenticate", content);
        }

        public async Task Logout()
        {
            await GetAsync("api/v1/logout");
        }

        public async Task<User> GetUserCurrent()
        {
            return await GetAsync<User>("api/v1/user/me");
        }


        public async Task<Organization> CreateOrganization(CreateOrganization content)
        {
            return await PostAsync<Organization>("api/v1/organization", content);
        }

        public async Task<Organization> GetOrganizationById(string organizationId)
        {
            return await GetAsync<Organization>($"api/v1/organization/{organizationId}");
        }

        public async Task<List<Organization>> GetCompaniesByUser(string userId)
        {
            return await GetAsync<List<Organization>>($"api/v1/organization/user/{userId}");
        }

        public async Task<InstanceTag> CreateTagByOrganizationId(string organizationId, string name, string backgroundColor, string fontColor)
        {
            CreateTag content = new CreateTag()
            {
                Name = name,
                BackgroundColor = backgroundColor,
                FontColor = fontColor
            };
            await PostAsync($"api/v1/organization/{organizationId}/tag", content);

            InstanceTag tag = new InstanceTag()
            {
                Name = content.Name,
                BackgroundColor = content.BackgroundColor,
                FontColor = content.FontColor
            };

            return tag;
        }

        public async Task DeleteTagByOrganizationIdTagId(string organizationId, string tagId)
        {
            await DeleteAsync($"api/v1/organization/{organizationId}/tag/{tagId}");
        }

        public async Task<AuthorizedUser> AddAuthorizedUserToOrganization(string organizationId, string email)
        {
            AddRemoveOrganizationAuthorizedUser content = new AddRemoveOrganizationAuthorizedUser()
            {
                Email = email
            };
            return await PostAsync<AuthorizedUser>($"api/v1/organization/{organizationId}/user", content);
        }

        public async Task RemoveAuthorizedUserFromOrganization(string organizationId, string email)
        {
            AddRemoveOrganizationAuthorizedUser content = new AddRemoveOrganizationAuthorizedUser()
            {
                Email = email
            };
            await DeleteAsysnc($"api/v1/organization/{organizationId}/user", content);
        }

        public async Task ToggleAuthorizeRole(string organizationId, string email, bool enabled)
        {
            ToggleUserRole content = new ToggleUserRole()
            {
                Email = email,
                Enabled = enabled
            };
            await PutAsync<AuthorizedUser>($"api/v1/organization/{organizationId}/user/authorized", content);
        }

        public async Task ToggleOwnerRole(string organizationId, string email, bool enabled)
        {
            ToggleUserRole content = new ToggleUserRole()
            {
                Email = email,
                Enabled = enabled
            };
            await PutAsync<AuthorizedUser>($"api/v1/organization/{organizationId}/user/owner", content);
        }

        public async Task<InstanceType> CreateDataType(string organizationId, string name)
        {
            CreateDataType content = new CreateDataType()
            {
                Name = name
            };
            return await PostAsync<InstanceType>($"api/v1/organization/{organizationId}/instancetype", content);
        }

        public async Task<List<InstanceType>> GetDataTypeByOrganizationId(string organizationId)
        {
            return await GetAsync<List<InstanceType>>($"api/v1/organization/{organizationId}/instancetype");
        }

        public async Task<InstanceType> GetDataTypeByOrganizationIdTypeId(string organizationId, string typeId)
        {
            return await GetAsync<InstanceType>($"api/v1/organization/{organizationId}/instancetype/{typeId}");
        }

        public async Task<Field> CreateField(string organizationId, string typeId, Field field)
        {
            CreateField content = new CreateField()
            {
                Name = field.Name,
                Row = field.Row,
                Column = field.Column,
                ColumnSpan = field.ColumnSpan,
                Optional = field.Optional,
                Options = field.Options,
                Type = field.Type
            };
            return await PostAsync<Field>($"api/v1/organization/{organizationId}/instancetype/{typeId}/field", content);
        }

        public async Task<Dictionary<string, string>> CreateInstance(string organizationId, string typeId, Dictionary<string,string> content)
        {
            return await PostAsync<Dictionary<string, string>>($"api/v1/organization/{organizationId}/instancetype/{typeId}/instance", content);
        }

        public async Task<List<Dictionary<string, string>>> SearchInstance(string organizationId, string typeId)
        {
            return await GetAsync<List<Dictionary<string, string>>>($"api/v1/organization/{organizationId}/instancetype/{typeId}/instance/search");
        }



        //public async Task EditField(string organizationId, Field field)
        //{
        //    EditField content = new EditField()
        //    {
        //        Name = field.Name,
        //        Row = field.Row,
        //        Column = field.Column,
        //        ColumnSpan = field.ColumnSpan,
        //        CSS = field.CSS,
        //        Optional = field.Optional,
        //        Options = field.Options
        //    };
        //    await PutAsync($"api/v1/organization/{organizationId}/field/{field.Id}", content);
        //}


        #region HttpClient Methods
        private async Task GetAsync(string path)
        {
            await Send(HttpMethod.Get, path);
        }
        private async Task<T> GetAsync<T>(string path)
        {
            var response = await Send(HttpMethod.Get, path);
            T result = await ParseResponseObject<T>(response);
            return result;
        }


        private async Task PostAsync(string path, object content)
        {
            await Send(HttpMethod.Post, path, content);
        }
        private async Task<T> PostAsync<T>(string path, object content)
        {
            var response = await Send(HttpMethod.Post, path, content);
            return await ParseResponseObject<T>(response);
        }


        private async Task PutAsync(string path, object content)
        {
            await Send(HttpMethod.Put, path, content);
        }
        private async Task<T> PutAsync<T>(string path, object content)
        {
            var response = await Send(HttpMethod.Put, path, content);
            return await ParseResponseObject<T>(response);
        }


        private async Task DeleteAsync(string path)
        {
            await Send(HttpMethod.Delete, path);
        }
        private async Task DeleteAsysnc(string path, object content)
        {
            await Send(HttpMethod.Delete, path, content);
        }



        private async Task<HttpResponseMessage> Send(HttpMethod method, string path, object content = null)
        {
            var httpWebRequest = new HttpRequestMessage(method, path);

            if (content != null)
            {
                string json = JsonConvert.SerializeObject(content);
                StringContent postContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                httpWebRequest.Content = postContent;
            }

            httpWebRequest.Properties[WebAssemblyHttpMessageHandler.FetchArgs] = new
            {
                credentials = "include"
            };
            HttpResponseMessage response = await _client.SendAsync(httpWebRequest);

            if (response.IsSuccessStatusCode == false)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(responseContent) == false)
                    _toastService.ShowError(responseContent);
            }
            return response;
        }

        private async Task<T> ParseResponseObject<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            else
            {
                return default(T);
            }
        }

        #endregion
    }
}
