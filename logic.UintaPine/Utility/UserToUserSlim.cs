using model.Data.UintaPine;
using model.Shared.UintaPine;
using System;
using System.Collections.Generic;
using System.Text;

namespace logic.UintaPine.Utility
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
