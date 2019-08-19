using model.UintaPine.Api;
using model.UintaPine.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace model.UintaPine.ExtensionMethods
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
