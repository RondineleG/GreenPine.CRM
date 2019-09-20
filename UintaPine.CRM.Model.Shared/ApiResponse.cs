using System;
using System.Collections.Generic;
using System.Text;

namespace UintaPine.CRM.Model.Shared
{
    public class ApiResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; }
    }
}
