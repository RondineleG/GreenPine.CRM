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
            return Ok(response);
        }

        [Route("api/v1/company/user/{id}")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCompaniesByUser(string id)
        {
            UserSlim user = await _userLogic.GetUserSlimByIdAsync(User.Identity.Name);

            //Validate that you own the account that is being deleted.
            if (id != user?.Id)
                return BadRequest(new CompaniesByUser() { Message = "Invalid Permissions", Success = false });

            var companies = await _companyLogic.GetCompaniesByUser(user.Id);

            return Ok(companies);
        }

    }
}