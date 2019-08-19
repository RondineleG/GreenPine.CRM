using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace model.UintaPine.Utility
{
    public class Ping
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Environment { get; set; }
        public bool DB { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }

}
