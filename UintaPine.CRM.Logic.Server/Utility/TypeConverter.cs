using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UintaPine.CRM.Logic.Server.Utility
{
    public static class TypeConverter
    {
        public static T ConvertObject<T>(object source)
        {
            var jsondata = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(jsondata);
        }
    }
}
