using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UintaPine.CRM.Model.Server
{
    public class ApplicationSettings
    {
        public string Name { get; set; }
        public string DatabaseConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string SigningKey { get; set; }
    }
}
