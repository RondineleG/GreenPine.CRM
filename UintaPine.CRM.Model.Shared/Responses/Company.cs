using System;
using System.Collections.Generic;
using System.Text;
using UintaPine.CRM.Model.Shared.Enumerations;

namespace UintaPine.CRM.Model.Shared.Responses
{
    public class Company
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<AuthorizedUser> Users { get; set; } = new List<AuthorizedUser>();
        public List<CustomerTag> Tags { get; set; } = new List<CustomerTag>();
        public List<Field> Fields { get; set; } = new List<Field>();
    }

    public class AuthorizedUser
    {
        public string Email { get; set; }
        public bool Authorized { get; set; } = true;
        public bool Owner { get; set; } = false;
    }

    public class CustomerTag
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string BackgroundColor { get; set; }
        public string FontColor { get; set; }
    }

    public class Field
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public FieldType Type { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int ColumnSpan { get; set; }
        public string Options { get; set; }
        public string CSS { get; set; }
        public bool Optional { get; set; } = true;
    }
}
