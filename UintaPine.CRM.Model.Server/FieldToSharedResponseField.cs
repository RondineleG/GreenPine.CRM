using UintaPine.CRM.Model.Database;
using UintaPine.CRM.Model.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace UintaPine.CRM.Model.Server
{
    public static class FieldToSharedResponseField
    {
        public static Shared.Responses.Field ToSharedResponseField(this Field field)
        {
            return new Shared.Responses.Field()
            {
                Id = field.Id,
                Name = field.Name,
                Row = field.Row,
                Column = field.Column,
                ColumnSpan = field.ColumnSpan,
                Optional = field.Optional,
                Options = field.Options,
                Type = field.Type
            };
        }
    }
}
