using System;
using System.Collections.Generic;

namespace GreenPine.CRM.Model.Database
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        public int LockoutCount { get; set; } = 0;
        public DateTime? LockoutDateTime { get; set; } = null;

        public string RecoveryToken { get; set; }
        public DateTime? RecoveryTokenExpiration { get; set; }

        public List<string> Roles { get; set; } = new List<string>();
        public DateTime LastSignin { get; set; } = DateTime.Now;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;
    }
}
