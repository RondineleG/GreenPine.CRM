using data.UintaPine;
using System;
using System.Collections.Generic;
using System.Text;

namespace logic.UintaPine
{
    public class CompanyLogic
    {
        private MongoContext _db { get; set; }
        private UtilityLogic _utilityLogic { get; set; }
        public CompanyLogic(MongoContext context, UtilityLogic utilityLogic)
        {
            _db = context;
            _utilityLogic = utilityLogic;
        }
    }
}
