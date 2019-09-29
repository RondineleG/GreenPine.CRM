using System;
using System.Collections.Generic;
using System.Text;

namespace UintaPine.CRM.Model.Shared.Responses
{
    public class Company
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<AuthorizedUser> Users { get; set; } = new List<AuthorizedUser>();
        public List<string> Tags { get; set; } = new List<string>();
    }

    public class AuthorizedUser
    {
        public string Email { get; set; }
        public bool Authorized { get; set; } = true;
        public bool Owner { get; set; } = false;
    }
}
