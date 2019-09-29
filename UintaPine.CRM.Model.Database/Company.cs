using System;
using System.Collections.Generic;
using System.Text;
using UintaPine.CRM.Model.Shared;
using UintaPine.CRM.Model.Shared.Enumerations;

namespace UintaPine.CRM.Model.Database
{
    public class Company
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }

        public List<AuthorizedUser> Users { get; set; } = new List<AuthorizedUser>();
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public List<string> Tags { get; set; } = new List<string>();
    }

    public class AuthorizedUser
    {
        public string Email { get; set; }
        public bool Authorized { get; set; } = true;
        public bool Owner { get; set; } = false;
    }

    public class Customer
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Postal { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public List<string> Tags { get; set; } = new List<string>();
    }
}
