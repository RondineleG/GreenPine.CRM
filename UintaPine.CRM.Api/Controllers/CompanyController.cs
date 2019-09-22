using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UintaPine.CRM.Logic.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UintaPine.CRM.Model.Shared;

namespace UintaPine.CRM.Api.Controllers
{
    public class CompanyController : ControllerBase
    {
        private UserLogic _userLogic { get; set; }
        private CompanyLogic _companyLogic { get; set; }
        public CompanyController(UserLogic userlogic, CompanyLogic companyLogic)
        {
            _userLogic = userlogic;
            _companyLogic = companyLogic;
        }

        [Route("api/v1/company")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCompany([FromBody]CreateCompany model)
        {
            UserSlim user = await _userLogic.GetUserSlimByIdAsync(User.Identity.Name);
            
            //TODO: Field validation

            var response = await _companyLogic.CreateCompanyAsync(model.Name, user.Id);
            if (response == null)
                return BadRequest(new CompanySlim() { Message = "A company is already associated with this user", Success = false });
            else
                return Ok(response);
        }

        [Route("api/v1/company/user/current")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCompanyCurrentUser()
        {
            UserSlim user = await _userLogic.GetUserSlimByIdAsync(User.Identity.Name);

            var companies = await _companyLogic.GetCompanyByUser(user.Id);

            return Ok(companies);
        }

    }
}