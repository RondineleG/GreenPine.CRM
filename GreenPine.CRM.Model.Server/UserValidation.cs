using GreenPine.CRM.Model.Database;

namespace GreenPine.CRM.Model.Server
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
        public User User { get; set; }
        public UserValidationResponseCode Code { get; set; }
    }
}
