using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UintaPine.CRM.Logic.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UintaPine.CRM.Model.Server;
using UintaPine.CRM.Model.Shared;
using model.UintaPine.Utility;

namespace UintaPine.CRM.Api.Controllers
{
    public class UserController : Controller
    {
        private UserLogic _userHelper { get; set; }
        private UtilityLogic _utilityLogic { get; set; }
        private TokenLogic _tokenHelper { get; set; }
        private ApplicationSettings _applicationSettings { get; set; }
        public UserController(UserLogic userHelper, UtilityLogic utilityLogic, TokenLogic tokenHelper, ApplicationSettings applicationSettings)
        {
            _userHelper = userHelper;
            _utilityLogic = utilityLogic;
            _tokenHelper = tokenHelper;
            _applicationSettings = applicationSettings;
        }

        [HttpPost]
        [Route("api/v1/authenticate")]
        async public Task<IActionResult> Authenticate([FromBody]Authenticate model)
        {
            //Get values from request, sent by client.
            string username = model.Email?.ToLower()?.Trim();
            string password = model.Password?.Trim();

            //Client validation passed.  Validate credentials.
            //Does the user have a valid account and did they provide a valid username/password.
            //Does user have valid credentials
            UserValidation validation = await _userHelper.ValidateUserAndUpdateIdentityAsync(username, password);
            if (validation.Code == UserValidationResponseCode.Invalid)
            {
                return BadRequest(new UserSlim() { Success = false, Message = "Invalid Username or Password" });
            }
            else if (validation.Code == UserValidationResponseCode.LockedOut)
            {
                return BadRequest(new UserSlim() { Success = false, Message = "Account is Locked. Wait 30 minutes." });
            }
            else if (validation.Code == UserValidationResponseCode.Invalidated)
            {
                return BadRequest(new UserSlim() { Success = false, Message = "Email has not been validated" });
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_applicationSettings.SigningKey));
            TokenProviderOptions options = new TokenProviderOptions()
            {
                Issuer = this.Request.Host.Value,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };
            //Client, Tokens, and User validation have all passed.  Build the tokens and response object
            string encodedJwt = await _tokenHelper.BuildJwtAuthorizationToken(validation.User, options);

            await _userHelper.UserSetLastSignInAsync(validation.User.Id);
            _tokenHelper.BuildResponseCookieSignIn(Request.HttpContext, encodedJwt);

            return Ok(validation.User);
        }



        [Route("api/v1/logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            _tokenHelper.BuildResponseCookieSignOut(Request.HttpContext);
            return Ok();
        }

        [HttpPost]
        [Route("api/v1/user")]
        async public Task<IActionResult> CreateUser([FromBody]Register model)
        {
            if (model.Email == null || model.Email == "")
                return BadRequest(new UserSlim() { Success = false, Message = "Email is required" });

            if (_utilityLogic.Email(model.Email) == false)
                return BadRequest(new UserSlim() { Success = false, Message = "Valid email address is required" });

            UserSlim existing = await _userHelper.GetUserByEmailAsync(model.Email);
            if (existing != null)
                return BadRequest(new UserSlim() { Success = false, Message = "Email address already used." });

            if (model.Password == null || model.Password == "")
                return BadRequest(new UserSlim() { Success = false, Message = "Password is required" });

            if (model.Password != model.ConfirmPassword)
                return BadRequest(new UserSlim() { Success = false, Message = "Passwords do not match" });

            if (!_userHelper.IsValidPassword(model.Password))
                return BadRequest(new UserSlim() { Success = false, Message = "Password is not complex enough." });

            UserSlim user = await _userHelper.CreateUserAsync(model.Email, model.Password);
            if (user != null)
            {
                //Use this if you want to send an activation email.
                //await _emailHelper.SendActivationEmailAsync(user);
                return Ok(user);
            }
            else
                return BadRequest(new UserSlim() { Success = false, Message = "Could not create user profile." });

        }

        [HttpGet]
        [Route("api/v1/user/{id}")]
        [Authorize]
        async public Task<IActionResult> GetUser(string id)
        {
            UserSlim user = await _userHelper.GetUserSlimByIdAsync(User.Identity.Name);
            if (user == null)
                return NotFound(new UserSlim() { Success = false, Message = "User Not Found" });

            //Remove this if you want to allow user info to be requested by others users.
            if (id != user?.Id && id != "me")
                return BadRequest(new UserSlim() { Success = false, Message = "Invalid Permissions" });

            if (user != null)
            {
                return Ok(user);
            }
            else
                return NotFound(new UserSlim() { Success = false, Message = "User Not Found" });
        }



        [Route("api/v1/user/{id}/password")]
        [HttpPut]
        [Authorize]
        async public Task<IActionResult> ChangePassword(string id, [FromBody]ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                UserSlim user = await _userHelper.GetUserSlimByIdAsync(User.Identity.Name);

                //Ensure that the requesting user is changing their own password. Change this to allow administrators to modify passwords.
                if (user?.Id != id)
                    return BadRequest("Invalid Permissions");

                if (model.NewPassword == null || model.NewPassword == "")
                    return BadRequest("Password is required");

                //Password confirm must match
                if (model.NewPassword != model.ConfirmPassword)
                    return BadRequest("Password and Confirmation must match");

                //Validate that the password meets the complexity requirements.
                if (!_userHelper.IsValidPassword(model.NewPassword))
                    return BadRequest("Password is not strong enough");

                //Ensure they passed the old password
                if (model.OldPassword == null)
                    return BadRequest("Invalid Password");

                //Check that they know the existing password
                bool validPassword = await _userHelper.PasswordMatch(user.Id, model.OldPassword);
                if (validPassword) { }
                else
                    return BadRequest("Invalid Password");

                //Change password is difficult, just remove the password and then add a new password based on the user input.
                string newSalt = _userHelper.CreatUserSalt();
                string newPasswordHash = _utilityLogic.GetHash(model.NewPassword + newSalt);

                await _userHelper.ChangePasswordAsync(user.Id, newSalt, newPasswordHash);

                return Ok();
            }
            else
                return BadRequest(ModelState);
        }




        [Authorize]
        [Route("api/v1/user/{id}")]
        [HttpPut]
        async public Task<IActionResult> UpdateAccount(string id, [FromBody]AccountUpdate model)
        {
            if (ModelState.IsValid)
            {
                UserSlim user = await _userHelper.GetUserSlimByIdAsync(User.Identity.Name);

                //Modify this if you want to allow user data to be requested that doesn't belog to the reques
                if (id != user?.Id)
                    return BadRequest("Invalid Permissions");

                if (model.Email == null || model.Email == "")
                    return BadRequest("Email address is required");

                if (_utilityLogic.Email(model.Email) == false)
                    return BadRequest("Invalid Email address");
                //Check if the email is already used by an existing user.  If so, return conflict.
                UserSlim existing = await _userHelper.GetUserByEmailAsync(model.Email);
                if (existing != null && user.Email != model.Email)
                    return BadRequest("Email address is used by another account");

                await _userHelper.UpdateUserAsync(id, model.Email);

                return Ok();
            }
            else
                return BadRequest();
        }

        [Authorize]
        [Route("api/v1/user/{id}")]
        [HttpDelete]
        async public Task<IActionResult> DeleteUser(string id)
        {
            if (ModelState.IsValid)
            {
                UserSlim user = await _userHelper.GetUserSlimByIdAsync(User.Identity.Name);

                //Validate that you own the account that is being deleted.
                if (id != user?.Id)
                    return BadRequest("Invalid Permissions");

                //Delete the user profile.
                await _userHelper.DeleteUserAsync(user.Id);

                Response.Cookies.Delete("access_token");
                return Ok();
            }
            else
                return BadRequest(ModelState);
        }


        [HttpPost]
        [Route("api/v1/user/recovery")]
        async public Task<IActionResult> PasswordRecovery([FromBody]ForgotPassword model)
        {
            ///
            /// User submits, system checks for username, if the username exists, it emails the user the reset key/email.
            /// If the username does not exist, the user will still see the same message, this is to ensure that somebody
            /// doesn't just attempt to guess the username/email.
            /// Password Recovery emails are sent via the UPQ.
            ///
            if (ModelState.IsValid)
            {
                if (model.Email == null || model.Email == "")
                    return BadRequest("Email is required");

                UserSlim user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    //Use this if youw wanto send a recovery email
                    //await _emailHelper.SendRecoveryEmailAsync(user);
                    return Ok();//Do not return the recoveryToken in the service.  Send a recovery email to validate the users ownership of the account.
                }
                else
                    return BadRequest("No account with the specified email address");
            }
            else
                return BadRequest();
        }

        [HttpPost]
        [Route("api/v1/user/recovery/{token}")]
        async public Task<IActionResult> PasswordReset(string token, [FromBody]ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                if (model.NewPassword == null || model.NewPassword == "")
                    return BadRequest("Password is required");

                if (model.NewPassword != model.ConfirmPassword)
                    return BadRequest("Passwords do not match");

                if (!_userHelper.IsValidPassword(model.NewPassword))
                    return BadRequest("Password is not complex enough.");

                try
                {
                    UserSlim user = await _userHelper.ValidateRecoveryToken(token);
                    if (user == null)
                        return BadRequest("Invalid Token");

                    await _userHelper.UpdateUserPasswordByUserIdAsync(user.Id, model.NewPassword);
                }
                catch
                {
                    return BadRequest();
                }

                return Ok("Password changed.  Sign in to continue using the application.");
            }
            else
                return BadRequest(ModelState);
        }

    }
}