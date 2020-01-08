using Microsoft.AspNetCore.Mvc;
using Flurl;
using Flurl.Http;

namespace Html2PdfChromium.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutBoundIP : Controller
    {
        private class PublicIp
        {
            public string ip = null;
        }
        
        // GET api/Html2Pdf
        [HttpGet]
        public ActionResult<string> Get()
        {
            var result = "https://api.ipify.org?format=json".GetJsonAsync<PublicIp>().Result;
            return result.ip;
        }
    }
}