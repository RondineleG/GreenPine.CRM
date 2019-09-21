using System;
using System.Collections.Generic;
using System.Text;

namespace UintaPine.CRM.Model.Database
{
    public class Company
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// OwnerId is the User.Id of the user who created, and therefore owns this company
        /// </summary>
        public string OwnerId { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Email address of users who are authorized to access this company
        /// </summary>
        public List<string> AuthorizedUsers { get; set; } = new List<string>();

        public List<Customer> Customers { get; set; } = new List<Customer>();
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
    }

    public enum Gender
    {
        Male = 1,
        Female = 2
    }
}
