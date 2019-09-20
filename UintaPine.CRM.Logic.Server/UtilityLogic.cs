using UintaPine.CRM.Database;
using UintaPine.CRM.Model.Database;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UintaPine.CRM.Logic.Server
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

        async public Task<Ping> PingAsync(string environment)
        {
            Ping ping = new Ping();
            ping.Environment = environment;

            try
            {
                _db.Pings.InsertOne(ping);
                ping = await _db.Pings.Find(p => p.Id == ping.Id).FirstOrDefaultAsync();

                if (ping != null)
                {
                    ping.DB = true;
                    await _db.Pings.ReplaceOneAsync(p => p.Id == ping.Id, ping);
                }
                else
                    ping.DB = false;

                return ping;
            }
            catch
            {
                ping.DB = false;
                return ping;
            }

            //Delete any previous "pings". Don't want to bloat the DB. Remove this line to keep a running log of all pings.
            //await _db.Pings.DeleteManyAsync(p => p.Id != ping.Id);
        }
    }
}
