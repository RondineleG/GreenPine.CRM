using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.UintaPine.Pages
{
    public class AccountBase : PagesBase
    {
        public string Email { get; set; }

        public void Authenticate()
        {
            AppState.SetEmail(Email);
        }
    }
}
