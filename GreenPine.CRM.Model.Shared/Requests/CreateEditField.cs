using GreenPine.CRM.Model.Shared.Enumerations;

namespace GreenPine.CRM.Model.Shared.Requests
{
    public class CreateDataType
    {
        public string Name { get; set; }
    }

    public class CreateField
    {
        public string Name { get; set; }
        public FieldType Type { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int ColumnSpan { get; set; }
        public string Options { get; set; }
        public bool Optional { get; set; }
        public bool SearchShow { get; set; }
        public int SearchOrder { get; set; }
    }

    public class EditField
    {
        public string Name { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int ColumnSpan { get; set; }
        public string Options { get; set; }
        public bool Optional { get; set; }
        public bool SearchShow { get; set; }
        public int SearchOrder { get; set; }
    }
}
