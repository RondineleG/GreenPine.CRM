using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UintaPine.CRM.Logic.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UintaPine.CRM.Model.Shared;
using UintaPine.CRM.Model.Database;
using UintaPine.CRM.Model.Shared.Requests;
using UintaPine.CRM.Model.Server;

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
            User user = await _userLogic.GetUserByIdAsync(User.Identity.Name);
            
            //TODO: Field validation

            Company result = await _companyLogic.CreateCompanyAsync(model.Name, user.Id);
            if (result == null)
                return BadRequest("A company is already associated with this user");

            return Ok(result.ToSharedResponseCompany());
        }

        [Route("api/v1/company/user/{userId}")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCompanyByUser(string userId)
        {
            User user = await _userLogic.GetUserByIdAsync(User.Identity.Name);
            if (user.Id != userId)
                return BadRequest("Unauthorized User");

            var result = await _companyLogic.GetCompanyByUser(user.Id);
            if (result == null)
                return BadRequest("Company not found");

            return Ok(result.ToSharedResponseCompany());
        }

    }
}