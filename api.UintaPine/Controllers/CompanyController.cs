using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using logic.UintaPine;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using model.Server.UintaPine;

namespace api.UintaPine.Controllers
{
    public class CompanyController : ControllerBase
    {
        private CompanyLogic _companyLogic { get; set; }
        private ApplicationSettings _applicationSettings { get; set; }
        public CompanyController(CompanyLogic companyLogic, ApplicationSettings applicationSettings)
        {
            _companyLogic = companyLogic;
            _applicationSettings = applicationSettings;
        }
    }
}