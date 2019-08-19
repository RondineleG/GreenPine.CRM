using model.UintaPine.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace model.UintaPine.Utility
{
    public enum UserValidationResponseCode
    {
        Validated = 1,
        Invalid = 2,
        LockedOut = 3,
        Invalidated = 4
    }

    public class UserValidation
    {
        public UserSlim User { get; set; }
        public UserValidationResponseCode Code { get; set; }
    }
}
