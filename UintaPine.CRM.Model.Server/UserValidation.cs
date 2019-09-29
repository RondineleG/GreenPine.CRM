using UintaPine.CRM.Model.Database;
using UintaPine.CRM.Model.Shared;

namespace UintaPine.CRM.Model.Server
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
