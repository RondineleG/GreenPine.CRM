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


        public async Task<Company> CreateCompany(CreateCompany content)
        {
            return await PostAsync<Company>("api/v1/company", content);
        }

        public async Task<Company> GetCompanyById(string companyId)
        {
            return await GetAsync<Company>($"api/v1/company/{companyId}");
        }

        public async Task<List<Company>> GetCompaniesByUser(string userId)
        {
            return await GetAsync<List<Company>>($"api/v1/company/user/{userId}");
        }

        public async Task<CustomerTag> CreateTagByCompanyId(string companyId, string name, string backgroundColor, string fontColor)
        {
            CreateTag content = new CreateTag()
            {
                Name = name,
                BackgroundColor = backgroundColor,
                FontColor = fontColor
            };
            await PostAsync($"api/v1/company/{companyId}/tag", content);

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
            await DeleteAsync($"api/v1/company/{companyId}/tag/{tagId}");
        }

        public async Task<AuthorizedUser> AddAuthorizedUserToCompany(string companyId, string email)
        {
            AddRemoveCompanyAuthorizedUser content = new AddRemoveCompanyAuthorizedUser()
            {
                Email = email
            };
            return await PostAsync<AuthorizedUser>($"api/v1/company/{companyId}/user", content);
        }

        public async Task RemoveAuthorizedUserFromCompany(string companyId, string email)
        {
            AddRemoveCompanyAuthorizedUser content = new AddRemoveCompanyAuthorizedUser()
            {
                Email = email
            };
            await DeleteAsysnc($"api/v1/company/{companyId}/user", content);
        }

        public async Task ToggleAuthorizeRole(string companyId, string email, bool enabled)
        {
            ToggleUserRole content = new ToggleUserRole()
            {
                Email = email,
                Enabled = enabled
            };
            await PutAsync<AuthorizedUser>($"api/v1/company/{companyId}/user/authorized", content);
        }

        public async Task ToggleOwnerRole(string companyId, string email, bool enabled)
        {
            ToggleUserRole content = new ToggleUserRole()
            {
                Email = email,
                Enabled = enabled
            };
            await PutAsync<AuthorizedUser>($"api/v1/company/{companyId}/user/owner", content);
        }

        public async Task<DataType> CreateDataType(string companyId, string name)
        {
            CreateDataType content = new CreateDataType()
            {
                Name = name
            };
            return await PostAsync<DataType>($"api/v1/company/{companyId}/datatype", content);
        }

        public async Task<List<DataType>> GetDataTypeByCompanyId(string companyId)
        {
            return await GetAsync<List<DataType>>($"api/v1/company/{companyId}/datatype");
        }

        public async Task<Field> CreateField(string companyId, string typeId, Field field)
        {
            CreateField content = new CreateField()
            {
                Name = field.Name,
                Row = field.Row,
                Column = field.Column,
                ColumnSpan = field.ColumnSpan,
                CSS = field.CSS,
                Optional = field.Optional,
                Options = field.Options,
                Type = field.Type
            };
            return await PostAsync<Field>($"api/v1/company/{companyId}/datatype/{typeId}/field", content);
        }

        //public async Task EditField(string companyId, Field field)
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
        //    await PutAsync($"api/v1/company/{companyId}/field/{field.Id}", content);
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
