using GreenPine.CRM.Logic.Server;
using GreenPine.CRM.Model.Database;
using GreenPine.CRM.Model.Server;
using GreenPine.CRM.Model.Shared.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenPine.CRM.Api.Controllers
{
    public class OrganizationController : ControllerBase
    {
        private UserLogic _userLogic { get; set; }
        private OrganizationLogic _organizationLogic { get; set; }
        public OrganizationController(UserLogic userlogic, OrganizationLogic organizationLogic)
        {
            _userLogic = userlogic;
            _organizationLogic = organizationLogic;
        }

        [Route("api/v1/organization")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrganization([FromBody]CreateOrganization model)
        {
            User user = await _userLogic.GetUserByIdAsync(User.Identity.Name);
            
            //TODO: Field validation

            Organization result = await _organizationLogic.CreateOrganizationAsync(model.Name, user.Email);
            if (result == null)
                return BadRequest("A organization is already associated with this user");

            return Ok(result.ToSharedResponseOrganization());
        }

        [Route("api/v1/organization/{organizationId}")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrganizationById(string organizationId)
        {
            User user = await _userLogic.GetUserByIdAsync(User.Identity.Name);

            Organization organization = await _organizationLogic.GetOrganizationById(organizationId);

            return Ok(organization.ToSharedResponseOrganization());
        }

        [Route("api/v1/organization/user/{userId}")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCompaniesByUser(string userId)
        {
            User user = await _userLogic.GetUserByIdAsync(User.Identity.Name);
            if (user.Id != userId)
                return BadRequest("Unauthorized User");

            var result = await _organizationLogic.GetCompaniesByUserEmail(user.Email);

            return Ok(result.Select(c => c.ToSharedResponseOrganization()).ToList());
        }

        [Route("api/v1/organization/{organizationId}/tag")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTagByOrganizationId([FromBody]CreateTag model, string organizationId)
        {
            User user = await _userLogic.GetUserByIdAsync(User.Identity.Name);

            //TODO: Validation

            await _organizationLogic.CreateTag(organizationId, model.Name, model.BackgroundColor, model.FontColor);

            return Ok();
        }

        [Route("api/v1/organization/{organizationId}/tag/{tagId}")]
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteTagByOrganizationIdTagId(string organizationId, string tagId)
        {
            User user = await _userLogic.GetUserByIdAsync(User.Identity.Name);

            await _organizationLogic.DeleteTag(organizationId, tagId);

            return Ok();
        }

        [Route("api/v1/organization/{organizationId}/user")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddAuthorizedUserToOrganization([FromBody]AddRemoveOrganizationAuthorizedUser model, string organizationId)
        {
            AuthorizedUser newAuthorizedUser = await _organizationLogic.AddAuthorizedUserToOrganization(organizationId, model.Email);
            return Ok(newAuthorizedUser);
        }

        [Route("api/v1/organization/{organizationId}/user")]
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveAuthorizedUserFromOrganization([FromBody]AddRemoveOrganizationAuthorizedUser model, string organizationId)
        {
            await _organizationLogic.RemovedAuthorizedUserFromOrganization(organizationId, model.Email);
            return Ok();
        }

        [Route("api/v1/organization/{organizationId}/user/authorized")]
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> AuthorizedUserToggleAuthorizedRole([FromBody]ToggleUserRole model, string organizationId)
        {
            await _organizationLogic.AuthorizedUserToggleAuthorized(organizationId, model.Email, model.Enabled);
            return Ok();
        }

        [Route("api/v1/organization/{organizationId}/user/owner")]
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> AuthorizedUserToggleOwnerdRole([FromBody]ToggleUserRole model, string organizationId)
        {
            await _organizationLogic.AuthorizedUserToggleOwner(organizationId, model.Email, model.Enabled);
            return Ok();
        }

        [Route("api/v1/organization/{organizationId}/instancetype")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateDataType([FromBody]CreateDataType model, string organizationId)
        {
            var result = await _organizationLogic.CreateDataType(organizationId, model.Name);
            return Ok(result.ToSharedResponseDataType());
        }

        [Route("api/v1/organization/{organizationId}/instancetype")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDataType(string organizationId)
        {
            var result = await _organizationLogic.GetDataTypesByOrganizationId(organizationId);
            var response = result.Select(t => t.ToSharedResponseDataType()).ToList();
            return Ok(response);
        }

        [Route("api/v1/organization/{organizationId}/instancetype/{typeId}")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDataType(string organizationId, string typeId)
        {
            var result = await _organizationLogic.GetDataTypesByOrganizationIdTypeId(organizationId, typeId);
            var response = result.ToSharedResponseDataType();
            return Ok(response);
        }

        //[Route("api/v1/organization/{organizationId/instancetype/{typeId}")]
        //[HttpDelete]
        //[Authorize]
        //public async Task<IActionResult> DeleteDataType(string organizationId, string typeId)
        //{
        //    return Ok();
        //}


        [Route("api/v1/organization/{organizationId}/instancetype/{typeId}/field")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateField([FromBody]CreateField model, string organizationId, string typeId)
        {
            Field newField = await _organizationLogic.CreateField(organizationId, typeId, model.Name, model.Type, model.Row, model.Column, model.ColumnSpan, model.Options, model.Optional, model.SearchShow, model.SearchOrder);
            return Ok(newField.ToSharedResponseField());
        }

        [Route("api/v1/organization/{organizationId}/instancetype/{typeId}/field/{fieldId}")]
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> EditField([FromBody]EditField model, string organizationId, string typeId, string fieldId)
        {
            await _organizationLogic.UpdateField(organizationId, typeId, fieldId, model.Name, model.Row, model.Column, model.ColumnSpan, model.Options, model.Optional, model.SearchShow, model.SearchOrder);
            return Ok();
        }

        [Route("api/v1/organization/{organizationId}/instancetype/{typeId}/instance")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateInstance([FromBody]Dictionary<string,string> model, string organizationId, string typeId)
        {
            model.Add("InstanceId", Guid.NewGuid().ToString());
            model.Add("OrganizationId", organizationId);
            model.Add("TypeId", typeId);
            await _organizationLogic.CreateInstance(model);
            return Ok(model);
        }

        [Route("api/v1/organization/{organizationId}/instancetype/{typeId}/instance/search")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SearchInstances(string organizationId, string typeId)
        {
            try
            {
                var result = await _organizationLogic.SearchInstances(organizationId, typeId);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return Ok();
            }
        }

    }
}