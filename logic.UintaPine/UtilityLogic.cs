using data.UintaPine;
using model.UintaPine.Utility;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace logic.UintaPine
{
    public class UtilityLogic
    {
        private MongoContext _db { get; set; }
        public UtilityLogic(MongoContext context)
        {
            _db = context;
        }
        
        /// <summary>
        /// Generate a SHA256 hash, based on input string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = SHA256.Create();
            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
        }

        public bool Email(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        public bool PhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
        }
    }
}
