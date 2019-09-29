using System;
using System.Collections.Generic;
using System.Text;
using UintaPine.CRM.Model.Shared.Enumerations;

namespace UintaPine.CRM.Model.Shared.Responses
{
    public class Customer
    {
        public string Id { get; set; }
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
}
