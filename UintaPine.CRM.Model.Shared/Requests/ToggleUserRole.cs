using System;
using System.Collections.Generic;
using System.Text;

namespace UintaPine.CRM.Model.Shared.Requests
{
    public class ToggleUserRole
    {
        public string Email { get; set; }
        public bool Enabled { get; set; }
    }
}
