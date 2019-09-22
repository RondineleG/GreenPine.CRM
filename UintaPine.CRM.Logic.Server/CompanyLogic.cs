using UintaPine.CRM.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UintaPine.CRM.Model.Database;
using UintaPine.CRM.Logic.Server.Utility;
using UintaPine.CRM.Model.Shared;
using MongoDB.Driver;

namespace UintaPine.CRM.Logic.Server
{
    public class CompanyLogic
    {
        private MongoContext _db { get; set; }
        private UtilityLogic _utilityLogic { get; set; }
        public CompanyLogic(MongoContext context, UtilityLogic utilityLogic)
        {
            _db = context;
            _utilityLogic = utilityLogic;
        }

        async public Task<CompanySlim> CreateCompanyAsync(string companyName, string userId)
        {
            var existingCompany = await GetCompanyByUser(userId);
            if (existingCompany != null)
                return default(CompanySlim);

            Company company = new Company()
            {
                Name = companyName,
                Owners = new List<string>() { userId }
            };

            await _db.Companies.InsertOneAsync(company);

            CompanySlim newCompany = new CompanySlim();
            newCompany = TypeConverter.ConvertObject<CompanySlim>(company);

            return newCompany;
        }

        async public Task<CompanySlim> GetCompanyByUser(string userId)
        {
            var company = await _db.Companies.Find(c => c.Owners.Contains(userId) || c.Authorized.Contains(userId)).Project(c => new CompanySlim() 
                                    { 
                                        Id = c.Id,
                                        Name = c.Name,
                                        Owners = c.Owners,
                                        Authorized = c.Authorized
                                    }).FirstOrDefaultAsync();
            return company;
        }
    }
}
