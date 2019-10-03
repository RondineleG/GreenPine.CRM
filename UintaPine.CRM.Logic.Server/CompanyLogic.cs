using UintaPine.CRM.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using UintaPine.CRM.Model.Database;
using MongoDB.Driver;
using System.Linq;

namespace UintaPine.CRM.Logic.Server
{
    public class CompanyLogic
    {
        private MongoContext _db { get; set; }
        public CompanyLogic(MongoContext context)
        {
            _db = context;
        }

        async public Task<Company> CreateCompanyAsync(string companyName, string email)
        {
            Company company = new Company()
            {
                Name = companyName,
                Users = new List<AuthorizedUser>() { new AuthorizedUser() { Email = email } }
            };

            await _db.Companies.InsertOneAsync(company);

            return company;
        }

        async public Task<List<Company>> GetCompaniesByUser(string email)
        {
            var companies = await _db.Companies.Find(c => c.Users.Any(u => u.Email == email)).ToListAsync();
            return companies;
        }

        async public Task<Company> GetCompanyById(string companyId)
        {
            var company = await _db.Companies.Find(c => c.Id == companyId).FirstOrDefaultAsync();
            return company;
        }

        async public Task CreateTag(string companyId, string name, string backgroundColor, string fontColor)
        {
            CustomerTag tag = new CustomerTag()
            {
                Name = name,
                BackgroundColor = backgroundColor.ToLower(),
                FontColor = fontColor.ToLower()
            };
            var update = Builders<Company>.Update.Push(c => c.Tags, tag);
            await _db.Companies.UpdateOneAsync(c => c.Id == companyId, update);
            
            //        var filter = Builders<Company>.Filter.And(Builders<Company>.Filter.Eq(x => x.Id, companyId),
            //        Builders<Company>.Filter.ElemMatch(x => x.Products, p => p.Id == productId));

            //        var update = Builders<Company>.Update
            //                    .Set(model => model.Products[-1].Approved, true);
            //        await _db.Companies.UpdateOneAsync(filter, update);
        }


        async public Task DeleteTag(string companyId, string tagId)
        {
            var company = await GetCompanyById(companyId);
            var update = Builders<Company>.Update.PullFilter(c => c.Tags, t => t.Id == tagId);
            await _db.Companies.UpdateOneAsync(c => c.Id == companyId, update);
        }

        async public Task<List<CustomerTag>> GetTags(string companyId)
        {
            var tags = await _db.Companies.Find(c => c.Id == companyId).Project(c => c.Tags).FirstOrDefaultAsync();
            return tags;
        }
    }
}
