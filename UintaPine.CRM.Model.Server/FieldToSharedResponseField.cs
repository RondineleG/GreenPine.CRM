using GreenPine.CRM.Model.Database;

namespace GreenPine.CRM.Model.Server
{
    public static class FieldToSharedResponseField
    {
        public static Shared.Responses.Field ToSharedResponseField(this Field field)
        {
            return new Shared.Responses.Field()
            {
                Id = field.Id,
                Name = field.Name,
                Type = field.Type,
                Row = field.Row,
                Column = field.Column,
                ColumnSpan = field.ColumnSpan,
                Optional = field.Optional,
                Options = field.Options,
                SearchShow = field.SearchShow,
                SearchOrder = field.SearchOrder
            };
        }
    }
}
