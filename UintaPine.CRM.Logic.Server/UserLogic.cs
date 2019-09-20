using UintaPine.CRM.Database;
using UintaPine.CRM.Logic.Server.Utility;
using UintaPine.CRM.Model.Database;
using UintaPine.CRM.Model.Shared;
using model.UintaPine.Utility;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace UintaPine.CRM.Logic.Server
{
    public class UserLogic
    {
        private MongoContext _db { get; set; }
        private UtilityLogic _utilityLogic { get; set; }
        public UserLogic(MongoContext context, UtilityLogic utilityLogic)
        {
            _db = context;
            _utilityLogic = utilityLogic;
        }

        /// <summary>
        /// Retrieve user by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        async public Task<UserSlim> GetUserSlimByIdAsync(string id)
        {
            User user = await _db.Users.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (user != null)
                return user.ToUserSlim();
            else
                return default(UserSlim);
        }

        async internal Task<User> GetUserByIdAsync(string id)
        {
            User user = await _db.Users.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (user != null)
                return user;
            else
                return default(User);
        }

        /// <summary>
        /// Retrieve user by email address
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        async public Task<UserSlim> GetUserByEmailAsync(string email)
        {
            User user = await _db.Users.Find(u => u.Email == email).FirstOrDefaultAsync();
            if (user != null)
                return user.ToUserSlim();
            else
                return default(UserSlim);
        }

        /// <summary>
        /// Validate user and save to DB
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        async public Task<UserSlim> CreateUserAsync(string email, string password)
        {
            if (await GetUserByEmailAsync(email) != null) //User email address must be unique
                return default(UserSlim);
            else if (!IsValidPassword(password)) //User password must meet complexity requirements
                return default(UserSlim);

            User user = new User();
            user.Email = email.ToLower();
            user.Salt = CreatUserSalt();
            user.Password = _utilityLogic.GetHash(password + user.Salt);

            await _db.Users.InsertOneAsync(user);

            return user.ToUserSlim();
        }

        async public Task UpdateUserAsync(string id, string email)
        {
            var update = Builders<User>.Update
                                .Set(u => u.Email, email);

            await _db.Users.UpdateOneAsync(u => u.Id == id, update);
        }


        async public Task<UserValidation> ValidateUserAndUpdateIdentityAsync(string username, string password)
        {
            if (username != null)
                username = username.ToLower();
            User user = await _db.Users.Find(u => u.Email.ToLower() == username).FirstOrDefaultAsync();

            if (user == null)
            {
                return new UserValidation()
                {
                    Code = UserValidationResponseCode.Invalid
                };
            }
            //Uncomment this to only allow users with validated emails to sign in.
            //else if (user.EmailValidated == false)
            //{
            //    return new UserValidationResponse()
            //    {
            //        Code = UserValidationResponseCode.Invalidated
            //    };
            //}
            else if (PasswordMatch(user, password) == false)
            {
                user.LockoutCount++;
                var updateLockoutCount = Builders<User>.Update.Set(u => u.LockoutCount, user.LockoutCount);
                await _db.Users.UpdateOneAsync(u => u.Id == user.Id, updateLockoutCount);

                if (user.LockoutCount >= 10)
                {
                    user.LockoutDateTime = DateTime.UtcNow.ToUniversalTime();
                    var updateLockoutDate = Builders<User>.Update.Set(u => u.LockoutDateTime, user.LockoutDateTime);
                    await _db.Users.UpdateOneAsync(u => u.Id == user.Id, updateLockoutDate);

                    return new UserValidation()
                    {
                        Code = UserValidationResponseCode.LockedOut
                    };
                }

                return new UserValidation()
                {
                    Code = UserValidationResponseCode.Invalid
                };
            }
            else
            {
                if (user.LockoutCount >= 10 && user.LockoutDateTime <= DateTime.UtcNow.Subtract(new TimeSpan(0, 30, 0)).ToUniversalTime())
                {
                    user.LockoutCount = 0;
                    var updateLockoutCount = Builders<User>.Update.Set(u => u.LockoutCount, user.LockoutCount).Set(u => u.LockoutDateTime, null);
                    await _db.Users.UpdateOneAsync(u => u.Id == user.Id, updateLockoutCount);
                    return new UserValidation()
                    {
                        Code = UserValidationResponseCode.Validated,
                        User = user.ToUserSlim()
                    };
                }

                else if (user.LockoutCount >= 10 && user.LockoutDateTime > DateTime.UtcNow.Subtract(new TimeSpan(0, 30, 0)))
                {
                    return new UserValidation()
                    {
                        Code = UserValidationResponseCode.LockedOut
                    };
                }
                else
                {
                    if (user.LockoutCount >= 0)
                    {
                        user.LockoutCount = 0;
                        var updateLockoutCount = Builders<User>.Update.Set(u => u.LockoutCount, user.LockoutCount).Set(u => u.LockoutDateTime, null);
                        await _db.Users.UpdateOneAsync(u => u.Id == user.Id, updateLockoutCount);
                    }
                    return new UserValidation()
                    {
                        Code = UserValidationResponseCode.Validated,
                        User = user.ToUserSlim()
                    };
                }
            }
        }

        /// <summary>
        /// Test password for complexity 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool IsValidPassword(string password)
        {
            return true;
        }

        /// <summary>
        /// Compare password hashes
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        async public Task<bool> PasswordMatch(string userId, string password)
        {
            User user = await GetUserByIdAsync(userId);
            return PasswordMatch(user, password);
        }
        internal bool PasswordMatch(User user, string password)
        {
            string hash = _utilityLogic.GetHash(password + user.Salt);

            if (user.Password == hash)
                return true;
            else
                return false;
        }

        public string CreatUserSalt()
        {
            var random = RandomNumberGenerator.Create();// new RNGCryptoServiceProvider();

            // Maximum length of salt
            int max_length = 128;

            // Empty salt array
            byte[] salt = new byte[max_length];

            // Build the random bytes
            random.GetBytes(salt);//.GetNonZeroBytes(salt);

            // Return the string encoded salt
            return Convert.ToBase64String(salt); ;
        }

        async public Task UserSetLastSignInAsync(string userId)
        {
            var lastSignIn = Builders<User>.Update.Set(u => u.LastSignin, DateTime.UtcNow);
            await _db.Users.UpdateOneAsync(u => u.Id == userId, lastSignIn);
        }

        async public Task ChangePasswordAsync(string userId, string newSalt, string newPasswordHash)
        {
            var updatePasswordAndSalt = Builders<User>.Update
                                .Set(u => u.Salt, newSalt)
                                .Set(u => u.Password, newPasswordHash);

            await _db.Users.UpdateOneAsync(u => u.Id == userId, updatePasswordAndSalt);
        }

        async public Task ResetPasswordAsync(string userId, string newSalt, string newPasswordHash)
        {
            var updatePasswordAndSalt = Builders<User>.Update
                                .Set(u => u.Salt, newSalt)
                                .Set(u => u.Password, newPasswordHash);

            await _db.Users.UpdateOneAsync(u => u.Id == userId, updatePasswordAndSalt);
        }

        async public Task DeleteUserAsync(string userId)
        {
            await _db.Users.DeleteOneAsync(u => u.Id == userId);
        }

        async public Task<string> GetRecoveryTokenAsync(string userId)
        {
            User user = await GetUserByIdAsync(userId);
            if (user == null)
                return default(string);

            if (user.RecoveryToken != null && user.RecoveryTokenExpiration > DateTime.Now.ToUniversalTime())
                return user.RecoveryToken;
            else
            {
                user.RecoveryToken = Guid.NewGuid().ToString();
                user.RecoveryTokenExpiration = DateTime.Now.ToUniversalTime().AddHours(6);
                var update = Builders<User>.Update
                    .Set(u => u.RecoveryToken, user.RecoveryToken)
                    .Set(u => u.RecoveryTokenExpiration, user.RecoveryTokenExpiration);

                await _db.Users.UpdateOneAsync(u => u.Id == user.Id, update);
                return user.RecoveryToken;
            }
        }

        async public Task<UserSlim> ValidateRecoveryToken(string token)
        {
            User user = await _db.Users.Find(u => u.RecoveryToken == token).FirstOrDefaultAsync();
            if (user != null && user.RecoveryToken == token && user.RecoveryTokenExpiration >= DateTime.Now.ToUniversalTime())
            {
                var update = Builders<User>.Update
                    .Set(u => u.RecoveryToken, null)
                    .Set(u => u.RecoveryTokenExpiration, null);

                await _db.Users.UpdateOneAsync(u => u.Id == user.Id, update);
                return user.ToUserSlim();
            }

            if (user != null)
            {
                var cleanUp = Builders<User>.Update
                    .Set(u => u.RecoveryToken, null)
                    .Set(u => u.RecoveryTokenExpiration, null);

                await _db.Users.UpdateOneAsync(u => u.Id == user.Id, cleanUp);
            }
            return default(UserSlim);
        }
        async public Task UpdateUserPasswordByUserIdAsync(string userId, string password)
        {
            string newSalt = CreatUserSalt();
            string newPasswordHash = _utilityLogic.GetHash(password + newSalt);

            var updatePasswordAndSalt = Builders<User>.Update
                .Set(u => u.Salt, newSalt)
                .Set(u => u.Password, newPasswordHash);

            await _db.Users.UpdateOneAsync(u => u.Id == userId, updatePasswordAndSalt);
        }
    }
}
