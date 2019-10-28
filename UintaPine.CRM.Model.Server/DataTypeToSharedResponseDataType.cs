using UintaPine.CRM.Model.Database;
using UintaPine.CRM.Model.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace UintaPine.CRM.Model.Server
{
    public static class DataTypeToSharedResponseDataType
    {
        public static Shared.Responses.InstanceType ToSharedResponseDataType(this InstanceType dataType)
        {
            return new Shared.Responses.InstanceType()
            {
                Id = dataType.Id,
                Name = dataType.Name,
                Fields = dataType.Fields.Select(field => new Shared.Responses.Field()
                {
                    Id = field.Id,
                    Name = field.Name,
                    Type = field.Type,
                    Row = field.Row,
                    Column = field.Column,
                    ColumnSpan = field.ColumnSpan,
                    CSS = field.CSS,
                    Optional = field.Optional,
                    Options = field.Options
                }).ToList()
            };
        }
    }
}
