using System;
using System.Collections.Generic;
using System.Text;
using UintaPine.CRM.Model.Shared.Enumerations;

namespace UintaPine.CRM.Model.Shared.Requests
{
    public class CreateField
    {
        public string Name { get; set; }
        public FieldType Type { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int ColumnSpan { get; set; }
        public List<string> Options { get; set; } = new List<string>();
        public string CSS { get; set; }
        public bool Optional { get; set; }
    }
}
