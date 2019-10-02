using UintaPine.CRM.Model.Database;
using UintaPine.CRM.Model.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace UintaPine.CRM.Model.Server
{
    public static class CompanyToSharedResponseCompany
    {
        public static Shared.Responses.Company ToSharedResponseCompany(this Company company)
        {
            return new Shared.Responses.Company()
            {
                Id = company.Id,
                Name = company.Name,
                Tags = company.Tags.Select(t => new Shared.Responses.CustomerTag()
                {
                    Id = t.Id,
                    Name = t.Name,
                    BackgroundColor = t.BackgroundColor,
                    FontColor = t.FontColor
                }).ToList(),
                Users = company.Users.Select(c => new Shared.Responses.AuthorizedUser()
                {
                    Email = c.Email,
                    Authorized = c.Authorized,
                    Owner = c.Owner
                }).ToList()
            };
        }
    }
}
