using model.UintaPine.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.UintaPine.Pages
{
    public class AccountBase : PagesBase
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        async public Task Register()
        {
            Register register = new Register()
            {
                Email = Email,
                Password = Password,
                ConfirmPassword = ConfirmPassword
            };
            var response = await _api.RegisterUser(register);
            if (response.Success == true)
            {
                AppState.Email = response.Id;
            }
            else
            {
                //somethign else
            }
        }

        async public Task Authenticate()
        {
            Authenticate authenticate = new Authenticate()
            {
                Email = Email,
                Password = Password
            };
            var response = await _api.AuthenticateUser(Email, Password);
            if (response.Success == true)
            {
                AppState.Email = response.Email;
            }
            else
            {
                //somethign else
            }
        }
    }
}
