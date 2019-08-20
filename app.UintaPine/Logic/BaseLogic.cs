using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.UintaPine.Logic
{
    public class BaseLogic : ComponentBase
    {
        protected API _api { get; set; } = new API();
    }
}
