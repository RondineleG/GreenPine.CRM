using System;
using System.Collections.Generic;

namespace GreenPine.CRM.Model.Shared.Responses
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }

        public List<string> Roles { get; set; }
        public DateTime LastSignin { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
