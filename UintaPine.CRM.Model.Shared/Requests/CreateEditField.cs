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
        public string Options { get; set; }
        public string CSS { get; set; }
        public bool Optional { get; set; }
    }
}
