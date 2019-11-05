using GreenPine.CRM.Logic.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace GreenPine.CRM.Api.Controllers
{
    public class UtilityController : ControllerBase
    {
        private UtilityLogic _utilityLogic { get; set; }
        private IConfiguration _configuration { get; set; }
        public UtilityController(UtilityLogic utilityLogic, IConfiguration configuration)
        {
            _utilityLogic = utilityLogic;
            _configuration = configuration;
        }

        [Route("api/v1/ping")]
        [HttpGet]
        async public Task<IActionResult> Ping()
        {
            var result = await _utilityLogic.PingAsync(_configuration["Name"]);

            return Ok(result);
        }
    }
}