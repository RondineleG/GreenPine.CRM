using UintaPine.CRM.Model.Database;
using UintaPine.CRM.Model.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace UintaPine.CRM.Logic.Server.Utility
{
    public static class UserToUserSlim
    {
        public static UserSlim ToUserSlim(this User user)
        {
            return new UserSlim()
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
