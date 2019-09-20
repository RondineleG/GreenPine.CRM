using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UintaPine.CRM.Logic.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UintaPine.CRM.Api.Controllers
{
    public class CompanyController : ControllerBase
    {
        private CompanyLogic _companyLogic { get; set; }
        public CompanyController(CompanyLogic companyLogic)
        {
            _companyLogic = companyLogic;
        }
    }
}