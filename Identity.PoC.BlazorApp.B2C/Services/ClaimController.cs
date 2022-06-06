using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Identity.PoC.BlazorApp.B2C.Services
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ClaimController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ClaimController(IConfiguration configuration)
        {
            _config = configuration;
        }
        // GET: api/<ClaimController>
        [HttpGet]
        public IActionResult Get()
        {
            var responseProperties = new Dictionary<string, object>
              {
                { "CustomClaim", "ClaimAdded1" },
                { "CustomClaim2", "ClaimAdded2" }
              };

            return new JsonResult(responseProperties) { StatusCode = 200 };
        }

        // GET api/<ClaimController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ClaimController>
        [HttpPost]
        //public IActionResult Post([FromBody] JObject body)
             public IActionResult Post()
        {
            // Get the object id of the user that is signing in.
            //var objectId = body.GetValue("objectId").ToString();
            string secret = _config["AzureAD:EnrichClaimSecret"];
            var responseProperties = new Dictionary<string, object>
              {
                { "extension_33903e226c8a4610b44e2a2265b0e234_CustomClaim", "ClaimAdded" },
                { "extension_33903e226c8a4610b44e2a2265b0e234_CustomClaim2",secret }
              };

            return new JsonResult(responseProperties) { StatusCode = 200 };
        }

        // PUT api/<ClaimController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ClaimController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
