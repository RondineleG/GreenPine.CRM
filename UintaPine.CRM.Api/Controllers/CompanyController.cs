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

            Company result = await _companyLogic.CreateCompanyAsync(model.Name, user.Email);
            if (result == null)
                return BadRequest("A company is already associated with this user");

            return Ok(result.ToSharedResponseCompany());
        }

        [Route("api/v1/company/{companyId}")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCompanyById(string companyId)
        {
            User user = await _userLogic.GetUserByIdAsync(User.Identity.Name);

            Company company = await _companyLogic.GetCompanyById(companyId);

            return Ok(company.ToSharedResponseCompany());
        }

        [Route("api/v1/company/user/{userId}")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCompaniesByUser(string userId)
        {
            User user = await _userLogic.GetUserByIdAsync(User.Identity.Name);
            if (user.Id != userId)
                return BadRequest("Unauthorized User");

            var result = await _companyLogic.GetCompaniesByUserEmail(user.Email);

            return Ok(result.Select(c => c.ToSharedResponseCompany()).ToList());
        }

        [Route("api/v1/company/{companyId}/tag")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTagByCompanyId([FromBody]CreateTag model, string companyId)
        {
            User user = await _userLogic.GetUserByIdAsync(User.Identity.Name);

            //TODO: Validation

            await _companyLogic.CreateTag(companyId, model.Name, model.BackgroundColor, model.FontColor);

            return Ok();
        }

        [Route("api/v1/company/{companyId}/tag/{tagId}")]
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteTagByCompanyIdTagId(string companyId, string tagId)
        {
            User user = await _userLogic.GetUserByIdAsync(User.Identity.Name);

            await _companyLogic.DeleteTag(companyId, tagId);

            return Ok();
        }

        [Route("api/v1/company/{companyId}/user")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddAuthorizedUserToCompany([FromBody]AddRemoveCompanyAuthorizedUser model, string companyId)
        {
            AuthorizedUser newAuthorizedUser = await _companyLogic.AddAuthorizedUserToCompany(companyId, model.Email);
            return Ok(newAuthorizedUser);
        }

        [Route("api/v1/company/{companyId}/user")]
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveAuthorizedUserFromCompany([FromBody]AddRemoveCompanyAuthorizedUser model, string companyId)
        {
            await _companyLogic.RemovedAuthorizedUserFromCompany(companyId, model.Email);
            return Ok();
        }

        [Route("api/v1/company/{companyId}/user/authorized")]
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> AuthorizedUserToggleAuthorizedRole([FromBody]ToggleUserRole model, string companyId)
        {
            await _companyLogic.AuthorizedUserToggleAuthorized(companyId, model.Email, model.Enabled);
            return Ok();
        }

        [Route("api/v1/company/{companyId}/user/owner")]
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> AuthorizedUserToggleOwnerdRole([FromBody]ToggleUserRole model, string companyId)
        {
            await _companyLogic.AuthorizedUserToggleOwner(companyId, model.Email, model.Enabled);
            return Ok();
        }

        [Route("api/v1/company/{companyId}/datatype")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateDataType([FromBody]CreateDataType model, string companyId)
        {
            var result = await _companyLogic.CreateDataType(companyId, model.Name);
            return Ok(result.ToSharedResponseDataType());
        }

        [Route("api/v1/company/{companyId}/datatype")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDataType(string companyId)
        {
            var result = await _companyLogic.GetDataTypesByCompanyId(companyId);
            var response = result.Select(t => t.ToSharedResponseDataType()).ToList();
            return Ok(response);
        }

        //[Route("api/v1/company/{companyId}/datatype/{typeId}")]
        //[HttpDelete]
        //[Authorize]
        //public async Task<IActionResult> DeleteDataType(string companyId, string typeId)
        //{
        //    return Ok();
        //}


        [Route("api/v1/company/{companyId}/datatype/{typeId}/field")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateField([FromBody]CreateField model, string companyId, string typeId)
        {
            Field newField = await _companyLogic.CreateField(companyId, typeId, model.Name, model.Type, model.Row, model.Column, model.ColumnSpan, model.Options, model.CSS, model.Optional);
            return Ok(newField.ToSharedResponseField());
        }

        //[Route("api/v1/company/{companyId}/datatype/{typeid/field/{fieldId}")]
        //[HttpPut]
        //[Authorize]
        //public async Task<IActionResult> CreateField([FromBody]EditField model, string companyId, string fieldId)
        //{
        //    //await _companyLogic.UpdateField(companyId, fieldId, model.Name, model.Row, model.Column, model.ColumnSpan, model.Options, model.CSS, model.Optional);
        //    return Ok();
        //}
    }
}