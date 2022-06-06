using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Identity.PoC.BlazorApp.B2C.Services
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ClaimController : ControllerBase
    {
        // GET: api/<ClaimController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ClaimController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ClaimController>
        [HttpPost]
        public IActionResult Post([FromBody] JObject body)
        {
            // Get the object id of the user that is signing in.
            var objectId = body.GetValue("objectId").ToString();

            var responseProperties = new Dictionary<string, object>
              {
                { "CustomClaim", objectId },
                { "CustomClaim2", "ClaimAdded" }
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
