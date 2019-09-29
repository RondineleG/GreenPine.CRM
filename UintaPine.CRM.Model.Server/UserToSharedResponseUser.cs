using UintaPine.CRM.Model.Database;
using UintaPine.CRM.Model.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace UintaPine.CRM.Model.Server
{
    public static class UserToSharedResponseUser
    {
        public static Shared.Responses.User ToSharedResponseUser(this User user)
        {
            return new Shared.Responses.User()
            {
                Id = user.Id,
                Email = user.Email,
                Roles = user.Roles,
                Created = user.Created,
                Updated = user.Updated,
                LastSignin = user.LastSignin
            };
        }
    }
}
