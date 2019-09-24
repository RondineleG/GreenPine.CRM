using System;
using System.Collections.Generic;
using System.Text;

namespace UintaPine.CRM.Model.Shared
{
    #region Request Models
    public class CreateCompanyRequestModel
    {
        public string Name { get; set; }
    }
    #endregion



    #region Response Models
    public class CompanyResponseModel : ApiResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<AuthorizedUserResponseModel> Users { get; set; } = new List<AuthorizedUserResponseModel>();
        public List<string> Tags { get; set; } = new List<string>();
    }
    
    public class AuthorizedUserResponseModel
    {
        public string Email { get; set; }
        public bool Authorized { get; set; } = true;
        public bool Owner { get; set; } = false;
    }
    
    public class CustomerResponseModel : ApiResponse    
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderViewModel Gender { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Postal { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    public enum GenderViewModel
    {
        Male = 1,
        Female = 2
    }
    #endregion
}
