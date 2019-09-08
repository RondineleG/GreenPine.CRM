using model.UintaPine.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.UintaPine.Pages
{
    public class AccountBase : PageBase
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        async public Task Register()
        {
            var response = await _api.RegisterUser(Email, Password, ConfirmPassword);
            if (response.Success)
            {
                AppState.User = response;
            }
            else
            {
                //Show message
            }
        }

        async public Task Authenticate()
        {
            var response = await _api.AuthenticateUser(Email, Password);
            if (response?.Success == true)
            {
                AppState.User = response;
            }
            else
            {
                //Show message
            }
        }
    }
}
