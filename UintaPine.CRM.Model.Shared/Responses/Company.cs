using System;
using System.Collections.Generic;
using System.Text;
using UintaPine.CRM.Model.Shared.Enumerations;

namespace UintaPine.CRM.Model.Shared.Responses
{
    public class Organization
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<AuthorizedUser> Users { get; set; } = new List<AuthorizedUser>();
        public List<InstanceTag> Tags { get; set; } = new List<InstanceTag>();
    }

    public class AuthorizedUser
    {
        public string Email { get; set; }
        public bool Authorized { get; set; } = true;
        public bool Owner { get; set; } = false;
    }

    public class InstanceTag
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string BackgroundColor { get; set; }
        public string FontColor { get; set; }
    }

    public class InstanceType
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public List<Field> Fields { get; set; } = new List<Field>();
    }

    public class Field
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public FieldType Type { get; set; }
        public int Row { get; set; } = 1;
        public int Column { get; set; } = 1;
        public int ColumnSpan { get; set; } = 1;
        public string Options { get; set; }
        public bool Optional { get; set; } = true;
        public bool SearchShow { get; set; } = false;
        public int SearchOrder { get; set; } = 1;
    }
}
