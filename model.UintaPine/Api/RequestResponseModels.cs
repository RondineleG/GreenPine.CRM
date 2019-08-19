using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace model.UintaPine
{

    public enum UserValidationResponseCode
    {
        Validated = 1,
        Invalid = 2,
        LockedOut = 3,
        Invalidated = 4
    }

    public class UserValidationResponse
    {
        public User User { get; set; }
        public UserValidationResponseCode Code { get; set; }
    }

    public class ChangePasswordDataModel
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }

    public class AccountUpdateDataModel
    {

        public string Email { get; set; }

        public string Name { get; set; }
    }

    public class AuthenticateDataModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

    }

    public class RegisterDataModel
    {
        //public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordDataModel
    {
        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }


    public class ForgotPasswordDataModel
    {
        public string Email { get; set; }
    }

    public class ResendActivationDataModel
    {
        public string Email { get; set; }
    }

    public class TodoItemViewModel
    {
        public string Task { get; set; }
    }

    public class IdResponse
    {
        public string Id { get; set; }
    }

    public class MessageResponse
    {
        public string Message { get; set; }
    }

    public class UserSlim
    {
        public string Id { get; set; }
        public string Email { get; set; }

        public List<string> Roles { get; set; }
        public DateTime LastSignin { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
