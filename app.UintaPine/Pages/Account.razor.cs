using model.UintaPine.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.UintaPine.Pages
{
    public class AccountBase : PageBase
    {
        //Authentication bindable properties
        public string EmailAuthenticate { get; set; }
        public string PasswordAuthenticate { get; set; }
        public string MessageAuthentication { get; set; }

        //Registration bindable properties
        public string EmailRegister { get; set; }
        public string PasswordRegister { get; set; }
        public string ConfirmPasswordRegister { get; set; }
        public string MessageRegister { get; set; }

        async public Task Register()
        {
            var response = await _api.RegisterUser(EmailRegister, PasswordRegister, ConfirmPasswordRegister);
            if (response.Success)
            {
                AppState.User = response;
            }
            else
            {
                MessageRegister = response.Message;
            }
        }

        async public Task Authenticate()
        {
            var response = await _api.AuthenticateUser(EmailAuthenticate, PasswordAuthenticate);
            if (response?.Success == true)
            {
                AppState.User = response;
                Navigation.NavigateTo("customers");
            }
            else
            {
                MessageAuthentication = response.Message;
            }
        }
    }
}
