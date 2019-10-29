using UintaPine.CRM.Model.Database;
using UintaPine.CRM.Model.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace UintaPine.CRM.Model.Server
{
    public static class OrganizationToSharedResponseOrganization
    {
        public static Shared.Responses.Organization ToSharedResponseOrganization(this Organization organization)
        {
            return new Shared.Responses.Organization()
            {
                Id = organization.Id,
                Name = organization.Name,
                Tags = organization.Tags.Select(t => new Shared.Responses.InstanceTag()
                {
                    Id = t.Id,
                    Name = t.Name,
                    BackgroundColor = t.BackgroundColor,
                    FontColor = t.FontColor
                }).ToList(),
                Users = organization.Users.Select(c => new Shared.Responses.AuthorizedUser()
                {
                    Email = c.Email,
                    Authorized = c.Authorized,
                    Owner = c.Owner
                }).ToList()                
            };
        }
    }
}
