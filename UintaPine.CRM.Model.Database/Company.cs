using System;
using System.Collections.Generic;
using System.Text;

namespace UintaPine.CRM.Model.Database
{
    public class Company
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public List<string> Owners { get; set; } = new List<string>();
        public List<string> Authorized { get; set; } = new List<string>();

        public string Name { get; set; }
        public List<Customer> Customers { get; set; } = new List<Customer>();

        public List<string> Tags { get; set; } = new List<string>();
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

    public enum Gender
    {
        Male = 1,
        Female = 2
    }
}
