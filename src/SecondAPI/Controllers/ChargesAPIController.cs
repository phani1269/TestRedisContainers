using ChargesAPI.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using System.Text;
using System.Text.Json;

namespace ChargesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChargesAPIController : ControllerBase
    {
        private readonly IDataProtector _dataProtector;

        public ChargesAPIController(IDataProtectionProvider dataProtector)
        {
            _dataProtector = dataProtector.CreateProtector(nameof(SessionMiddleware));

        }

        [HttpGet]
        // To get the Session data from redis cache data
        public ActionResult GetCacheData()
        {
           var sessiondata = HttpContext.Session.GetString("Charges");
            return Ok(sessiondata);
        }
        [HttpPost]
        // to set the session data to redis cache
        public ActionResult SetCacheData(ChargeModel chargeModel)
        {
           var serial = JsonSerializer.Serialize<ChargeModel>(chargeModel);
            HttpContext.Session.SetString("Charges", serial);
            return Ok();
        }
        [HttpGet]
        [Route("[action]")]
        // to retrive session key from cookies
        public async Task<IActionResult> GetSessionKey()
        {
            HttpContext.Request.Cookies.TryGetValue("ChargesAPI_session", out string cookieValue);

            var protectedData = Convert.FromBase64String(Pad(cookieValue));

            var unprotectedData = _dataProtector.Unprotect(protectedData);

            var humanReadableData = System.Text.Encoding.UTF8.GetString(unprotectedData);

            return Ok(humanReadableData);
        }
        private string Pad(string text)
        {
            var padding = 3 - ((text.Length + 3) % 4);
            if (padding == 0)
            {
                return text;
            }
            return text + new string('=', padding);
        }
    }
}
