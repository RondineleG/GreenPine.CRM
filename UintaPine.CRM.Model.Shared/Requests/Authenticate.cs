using System;
using System.Collections.Generic;
using System.Text;

namespace UintaPine.CRM.Model.Shared.Requests
{
    public class Authenticate
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
