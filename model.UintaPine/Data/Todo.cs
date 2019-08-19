using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace model.UintaPine.Data
{
    public class Todo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Owner { get; set; }
        public string Task { get; set; }
        public bool Completed { get; set; }
    }
}
