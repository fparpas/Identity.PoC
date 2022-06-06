using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;

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

        // POST api/<ClaimController>
        [HttpPost]
        public IActionResult Post([FromBody] JsonElement input)
        //public IActionResult Post()
        {
            // Get the object id of the user that is signing in.
            JsonElement objectId = new JsonElement();
            input.TryGetProperty("objectId", out objectId);

            string secret = _config["AzureAD:EnrichClaimSecret"];
            string token = _config["AzureAD:EnrichClaimToken"];

            if (IsAuthorized(token, secret))
            {
                var responseProperties = new Dictionary<string, object>
              {
                { "extension_33903e226c8a4610b44e2a2265b0e234_CustomClaim", token },
                { "extension_33903e226c8a4610b44e2a2265b0e234_CustomClaim2","Custom Claim" },
                { "extension_33903e226c8a4610b44e2a2265b0e234_CustomObjectId",objectId.ToString() }
              };

                return new JsonResult(responseProperties) { StatusCode = 200 };
            }
            else
            {
                return Unauthorized();
            }
        }

        private bool IsAuthorized(string user, string secret)
        {
            string authHeader = Request.Headers["Authorization"].ToString();

            if (String.IsNullOrEmpty(authHeader))
            {
                return false;
            }
            else if (!authHeader.StartsWith("Basic "))
            {
                return false;
            }
            else
            {
                var inputs = authHeader.Split(" ");
                if (inputs == null || inputs.Count() != 2)
                {
                    return false;
                }
                else
                {
                    string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(inputs[1]));
                    if (String.IsNullOrEmpty(decoded))
                    {
                        return false;
                    }
                    else
                    {
                        var values = decoded.Split(":");
                        if (values == null || values.Count() != 2)
                        {
                            return false;
                        }
                        else
                        {
                            if (string.Compare(values[0], user, true) != 0)
                            {
                                return false;
                            }
                            else if (string.Compare(values[1], secret, false) != 0)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }

                        }
                    }
                }
            }
        }
    }
}
