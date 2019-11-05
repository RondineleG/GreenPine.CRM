using GreenPine.CRM.Model.Database;
using System.Linq;

namespace GreenPine.CRM.Model.Server
{
    public static class DataTypeToSharedResponseDataType
    {
        public static Shared.Responses.InstanceType ToSharedResponseDataType(this InstanceType dataType)
        {
            return new Shared.Responses.InstanceType()
            {
                Id = dataType.Id,
                Name = dataType.Name,
                Fields = dataType.Fields.Select(field => field.ToSharedResponseField()).ToList()
            };
        }
    }
}
