using System;
using System.Collections.Generic;
using System.Text;

namespace model.UintaPine.Api
{
    public class ResetPassword
    {
        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
