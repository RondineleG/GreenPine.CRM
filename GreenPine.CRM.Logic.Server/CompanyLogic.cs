using GreenPine.CRM.Database;
using GreenPine.CRM.Model.Database;
using GreenPine.CRM.Model.Shared.Enumerations;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenPine.CRM.Logic.Server
{
    public class OrganizationLogic
    {
        private MongoContext _db { get; set; }
        public OrganizationLogic(MongoContext context)
        {
            _db = context;
        }

        async public Task<Organization> CreateOrganizationAsync(string organizationName, string email)
        {
            Organization organization = new Organization()
            {
                Name = organizationName,
                Users = new List<AuthorizedUser>() { new AuthorizedUser() { Email = email, Owner = true } }
            };

            await _db.Companies.InsertOneAsync(organization);

            return organization;
        }

        async public Task<List<Organization>> GetCompaniesByUserEmail(string email)
        {
            var companies = await _db.Companies.Find(c => c.Users.Any(u => u.Email == email)).ToListAsync();
            return companies;
        }

        async public Task<Organization> GetOrganizationById(string organizationId)
        {
            var organization = await _db.Companies.Find(c => c.Id == organizationId).FirstOrDefaultAsync();
            return organization;
        }

        async public Task CreateTag(string organizationId, string name, string backgroundColor, string fontColor)
        {
            InstanceTag tag = new InstanceTag()
            {
                Name = name,
                BackgroundColor = backgroundColor.ToLower(),
                FontColor = fontColor.ToLower()
            };
            var update = Builders<Organization>.Update.Push(c => c.Tags, tag);
            await _db.Companies.UpdateOneAsync(c => c.Id == organizationId, update);
            
            
        }


        async public Task DeleteTag(string organizationId, string tagId)
        {
            var update = Builders<Organization>.Update.PullFilter(c => c.Tags, t => t.Id == tagId);
            await _db.Companies.UpdateOneAsync(c => c.Id == organizationId, update);
        }

        async public Task<List<InstanceTag>> GetTags(string organizationId)
        {
            var tags = await _db.Companies.Find(c => c.Id == organizationId).Project(c => c.Tags).FirstOrDefaultAsync();
            return tags;
        }

        async public Task<AuthorizedUser> AddAuthorizedUserToOrganization(string organizationId, string email)
        {
            //Do not allow a duplicate email address to be added as an authorized user.
            var organization = await GetOrganizationById(organizationId);
            if (organization.Users.FirstOrDefault(c => c.Email == email) != null)
                return default(AuthorizedUser);

            AuthorizedUser user = new AuthorizedUser()
            {
                Email = email.ToLower().Trim(),
                Authorized = true,
                Owner = false
            };
    
            var update = Builders<Organization>.Update.Push(c => c.Users, user);
            await _db.Companies.UpdateOneAsync(c => c.Id == organizationId, update);

            return user;
        }

        async public Task RemovedAuthorizedUserFromOrganization(string organizationId, string email)
        {
            var update = Builders<Organization>.Update.PullFilter(c => c.Users, t => t.Email == email);
            await _db.Companies.UpdateOneAsync(c => c.Id == organizationId, update);
        }

        async public Task AuthorizedUserToggleAuthorized(string organizationId, string email, bool enabled)
        {
            var filter = Builders<Organization>.Filter.And(Builders<Organization>.Filter.Eq(x => x.Id, organizationId),
            Builders<Organization>.Filter.ElemMatch(x => x.Users, p => p.Email == email));

            var update = Builders<Organization>.Update
                        .Set(model => model.Users[-1].Authorized, enabled);
            await _db.Companies.UpdateOneAsync(filter, update);
        }

        async public Task AuthorizedUserToggleOwner(string organizationId, string email, bool enabled)
        {
            var filter = Builders<Organization>.Filter.And(Builders<Organization>.Filter.Eq(x => x.Id, organizationId),
            Builders<Organization>.Filter.ElemMatch(x => x.Users, p => p.Email == email));

            var update = Builders<Organization>.Update
                        .Set(model => model.Users[-1].Owner, enabled);
            await _db.Companies.UpdateOneAsync(filter, update);
        }

        async public Task<InstanceType> CreateDataType(string organizationId, string name)
        {
            InstanceType newDataType = new InstanceType()
            {
                Name = name,
                OrganizationId = organizationId
            };
            await _db.Types.InsertOneAsync(newDataType);

            return newDataType;
        }

        async public Task<List<InstanceType>> GetDataTypesByOrganizationId(string organizationId)
        {
            var type = await _db.Types.Find(t => t.OrganizationId == organizationId).ToListAsync();
            return type;
        }

        async public Task<InstanceType> GetDataTypesByOrganizationIdTypeId(string organizationId, string typeId)
        {
            var type = await _db.Types.Find(t => t.OrganizationId == organizationId && t.Id == typeId).FirstOrDefaultAsync();
            return type;
        }

        async public Task<Field> CreateField(string organizationId, string typeId, string name, FieldType type, int row, int col, int colSpan, string options, bool optional, bool searchShow, int searchOrder)
        {
            Field newField = new Field()
            {
                Name = name,
                Type = type,
                Row = row,
                Column = col,
                ColumnSpan = colSpan,
                Options = options,
                Optional = optional,
                SearchShow = searchShow,
                SearchOrder = searchOrder
            };

            var update = Builders<InstanceType>.Update.Push(c => c.Fields, newField);
            await _db.Types.UpdateOneAsync(c => c.OrganizationId == organizationId && c.Id == typeId, update);

            return newField;
        }

        async public Task UpdateField(string organizationId, string typeId, string fieldId, string name, int row, int col, int colSpan, string options, bool optional, bool searchShow, int searchOrder)
        {
            var filter = Builders<InstanceType>.Filter.And(
                Builders<InstanceType>.Filter.Eq(x => x.OrganizationId, organizationId),
                Builders<InstanceType>.Filter.Eq(x => x.Id, typeId),
                Builders<InstanceType>.Filter.ElemMatch(x => x.Fields, p => p.Id == fieldId));

            var update = Builders<InstanceType>.Update
                        .Set(model => model.Fields[-1].Name, name)
                        .Set(model => model.Fields[-1].Row, row)
                        .Set(model => model.Fields[-1].Column, col)
                        .Set(model => model.Fields[-1].ColumnSpan, colSpan)
                        .Set(model => model.Fields[-1].Options, options)
                        .Set(model => model.Fields[-1].Optional, optional)
                        .Set(model => model.Fields[-1].SearchShow, searchShow)
                        .Set(model => model.Fields[-1].SearchOrder, searchOrder);
            await _db.Types.UpdateOneAsync(filter, update);
        }

        async public Task CreateInstance(Dictionary<string, string> data)
        {
            await _db.Instances.InsertOneAsync(data);
        }

        async public Task<List<Dictionary<string,string>>> SearchInstances(string companyId, string typeId)
        {
            //Query used in data results and count results. Separate the query from the rest of the pipeline so it can be reused.
            var query = new BsonDocument("$and",
                        new BsonArray
                        {
                            new BsonDocument("OrganizationId", companyId),
                            new BsonDocument("TypeId", typeId)
                        });

            PipelineDefinition<Dictionary<string,string>, Dictionary<string, string>> pipelineData = new BsonDocument[]
            {
                new BsonDocument("$match", query),
                new BsonDocument("$project",
                new BsonDocument
                    {
                        { "_id", 0 },
                    })
            };

            //Search
            var response = await _db.Instances.Aggregate(pipelineData).ToListAsync();

            return response;
        }

        //async public Task DeleteField(string organizationId, string fieldId)
        //{

        //}
    }
}
