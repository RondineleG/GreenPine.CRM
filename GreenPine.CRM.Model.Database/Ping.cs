using System;

namespace GreenPine.CRM.Model.Database
{
    public class Ping
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Environment { get; set; }
        public bool DB { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }

}
