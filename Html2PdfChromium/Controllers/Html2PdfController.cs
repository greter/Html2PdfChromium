using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace Html2PdfChromium.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Html2PdfController : ControllerBase
    {
        // GET api/Html2Pdf
        [HttpGet]
        public ActionResult<Status> Get()
        {
            return new Status(Status.Ok);
        }
        
        //POST api/Html2Pdf
        [HttpPost]
        public async Task<IActionResult> CreatePdfSync([FromBody] PdfParameters parameters)
        {
            var pdf = await CreatePdf.CreatePdfSync(parameters);
            return new FileStreamResult(pdf, "application/pdf") {FileDownloadName = "document.pdf"};
        }
    }
}