using System;
using System.Collections.Generic;
using System.Text;

namespace UintaPine.CRM.Model.Shared
{
    /*
     * What is a "Slim" class? A smaller version of the same class, usually found in the database model.
     */


    public class CreateCompany
    {
        public string Name { get; set; }
    }

    

    public class CompanySlim : ApiResponse
    {
        public string Id { get; set; }
        public List<string> Owners { get; set; } = new List<string>();
        public List<string> Authorized { get; set; } = new List<string>();
        public string Name { get; set; }
    }

    public class CustomerSlim
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderSlim Gender { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Postal { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    public enum GenderSlim
    {
        Male = 1,
        Female = 2
    }
}
