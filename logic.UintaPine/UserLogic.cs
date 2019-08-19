using data.damn;
using model.damn;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace logic.UintaPine
{
    public class UserLogic
    {
        private MongoContext _db { get; set; }
        private HasherLogic _hasherHelper { get; set; }
        public UserLogic(MongoContext context, HasherLogic hasherHelper)
        {
            _db = context;
            _hasherHelper = hasherHelper;
        }

        /// <summary>
        /// Retrieve user by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        async public Task<User> GetUserByIdAsync(string id)
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
        async public Task<User> GetUserByEmailAsync(string email)
        {
            User user = await _db.Users.Find(u => u.Email == email).FirstOrDefaultAsync();
            if (user != null)
                return user;
            else
                return default(User);
        }

        /// <summary>
        /// Validate user and save to DB
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        async public Task<bool> CreateUserAsync(User user)
        {
            if (await GetUserByEmailAsync(user.Email) != null) //User email address must be unique
                return false;
            else if (!IsValidPassword(user.Password)) //User password must meet complexity requirements
                return false;

            await _db.Users.InsertOneAsync(user);

            return true;
        }

        async public Task UpdateUserAsync(string id, string email, string name)
        {
            var update = Builders<User>.Update
                                .Set(u => u.Email, email)
                                .Set(u => u.Name, name);

            await _db.Users.UpdateOneAsync(u => u.Id == id, update);
        }

        public UserSlim UserToUserSlim(User user)
        {
            return new UserSlim()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                LastSignin = user.LastSignin,
                Created = user.Created,
                Updated = user.Updated,
                Roles = user.Roles
            };
        }

        async public Task<UserValidationResponse> ValidateUserIdentityAsync(string username, string password)
        {
            if (username != null)
                username = username.ToLower();
            User user = await _db.Users.Find(u => u.Email.ToLower() == username).FirstOrDefaultAsync();

            if (user == null)
            {
                return new UserValidationResponse()
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

                    return new UserValidationResponse()
                    {
                        Code = UserValidationResponseCode.LockedOut
                    };
                }

                return new UserValidationResponse()
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
                    return new UserValidationResponse()
                    {
                        Code = UserValidationResponseCode.Validated,
                        User = user
                    };
                }

                else if (user.LockoutCount >= 10 && user.LockoutDateTime > DateTime.UtcNow.Subtract(new TimeSpan(0, 30, 0)))
                {
                    return new UserValidationResponse()
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
                    return new UserValidationResponse()
                    {
                        Code = UserValidationResponseCode.Validated,
                        User = user
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
        public bool PasswordMatch(User user, string password)
        {
            string hash = _hasherHelper.GetHash(password + user.Salt);

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

        async public Task UserSetLastSignInAsync(User user)
        {
            var lastSignIn = Builders<User>.Update.Set(u => u.LastSignin, DateTime.UtcNow);
            await _db.Users.UpdateOneAsync(u => u.Id == user.Id, lastSignIn);
        }

        async public Task ChangePasswordAsync(User user, string newSalt, string newPasswordHash)
        {
            var updatePasswordAndSalt = Builders<User>.Update
                                .Set(u => u.Salt, newSalt)
                                .Set(u => u.Password, newPasswordHash);

            await _db.Users.UpdateOneAsync(u => u.Id == user.Id, updatePasswordAndSalt);
        }

        async public Task ResetPasswordAsync(User user, string newSalt, string newPasswordHash)
        {
            var updatePasswordAndSalt = Builders<User>.Update
                                .Set(u => u.Salt, newSalt)
                                .Set(u => u.Password, newPasswordHash);

            await _db.Users.UpdateOneAsync(u => u.Id == user.Id, updatePasswordAndSalt);
        }

        async public Task DeleteUserAsync(User user)
        {
            await _db.Users.DeleteOneAsync(u => u.Id == user.Id);
        }

        async public Task<string> GetRecoveryTokenAsync(User user)
        {
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

        async public Task<User> ValidateRecoveryToken(string token)
        {
            User user = await _db.Users.Find(u => u.RecoveryToken == token).FirstOrDefaultAsync();
            if (user != null && user.RecoveryToken == token && user.RecoveryTokenExpiration >= DateTime.Now.ToUniversalTime())
            {
                var update = Builders<User>.Update
                    .Set(u => u.RecoveryToken, null)
                    .Set(u => u.RecoveryTokenExpiration, null);

                await _db.Users.UpdateOneAsync(u => u.Id == user.Id, update);
                return user;
            }

            if (user != null)
            {
                var cleanUp = Builders<User>.Update
                    .Set(u => u.RecoveryToken, null)
                    .Set(u => u.RecoveryTokenExpiration, null);

                await _db.Users.UpdateOneAsync(u => u.Id == user.Id, cleanUp);
            }
            return default(User);
        }

        async public Task<string> GetActivationTokenAsync(User user)
        {
            if (user.ActivationToken != null && user.ActivationTokenExpiration > DateTime.Now.ToUniversalTime())
                return user.ActivationToken;
            else
            {
                user.ActivationToken = Guid.NewGuid().ToString();
                user.ActivationTokenExpiration = DateTime.Now.ToUniversalTime().AddDays(2);
                var update = Builders<User>.Update
                    .Set(u => u.ActivationToken, user.ActivationToken)
                    .Set(u => u.ActivationTokenExpiration, user.ActivationTokenExpiration);

                await _db.Users.UpdateOneAsync(u => u.Id == user.Id, update);
                return user.ActivationToken;
            }
        }

        async public Task<User> ValidateActivationToken(string token)
        {
            User user = await _db.Users.Find(u => u.ActivationToken == token).FirstOrDefaultAsync();
            if (user != null && user.ActivationToken == token && user.ActivationTokenExpiration >= DateTime.Now.ToUniversalTime())
            {
                var update = Builders<User>.Update
                    .Set(u => u.ActivationToken, null)
                    .Set(u => u.ActivationTokenExpiration, null);

                await _db.Users.UpdateOneAsync(u => u.Id == user.Id, update);
                return user;
            }

            if (user != null)
            {
                var cleanUp = Builders<User>.Update
                    .Set(u => u.ActivationToken, null)
                    .Set(u => u.ActivationTokenExpiration, null);

                //await _db.Users.UpdateOneAsync(u => u.Id == user.Id, cleanUp);
            }
            return default(User);
        }

        async public Task ActivateUser(User user)
        {
            var update = Builders<User>.Update
                .Set(u => u.ActivationToken, null)
                .Set(u => u.ActivationTokenExpiration, null)
                .Set(u => u.EmailValidated, true);

            await _db.Users.UpdateOneAsync(u => u.Id == user.Id, update);
        }

        async public Task UpdateUserPasswordByUserIdAsync(string userId, string password)
        {
            string newSalt = CreatUserSalt();
            string newPasswordHash = _hasherHelper.GetHash(password + newSalt);

            var updatePasswordAndSalt = Builders<User>.Update
                .Set(u => u.Salt, newSalt)
                .Set(u => u.Password, newPasswordHash)
                .Set(u => u.ActivationToken, null)
                .Set(u => u.ActivationTokenExpiration, null)
                .Set(u => u.EmailValidated, true);

            await _db.Users.UpdateOneAsync(u => u.Id == userId, updatePasswordAndSalt);
        }
    }
}
