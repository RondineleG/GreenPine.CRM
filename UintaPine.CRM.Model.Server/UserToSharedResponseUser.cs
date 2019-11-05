using GreenPine.CRM.Model.Database;

namespace GreenPine.CRM.Model.Server
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
