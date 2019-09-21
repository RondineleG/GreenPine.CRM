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

        async public Task<CompanySlim> CreateCompanyAsync(string companyName, string ownerId)
        {
            Company company = new Company()
            {
                Name = companyName,
                OwnerId = ownerId
            };

            await _db.Companies.InsertOneAsync(company);

            CompanySlim newCompany = new CompanySlim();
            newCompany = TypeConverter.ConvertObject<CompanySlim>(company);

            return newCompany;
        }

        async public Task<List<CompanySlim>> GetCompaniesByUser(string userId)
        {
            var companies = await _db.Companies.Find(c => c.OwnerId == userId).Project(c => new CompanySlim() 
                                    { 
                                        Id = c.Id,
                                        Name = c.Name,
                                        OwnerId = c.OwnerId,
                                        AuthorizedUsers = c.AuthorizedUsers
                                    }).ToListAsync();

            return companies;
        }
    }
}
