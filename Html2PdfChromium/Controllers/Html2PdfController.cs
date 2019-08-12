using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;

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
        public async Task<IActionResult> PolicyStatement([FromBody] PdfParameters parameters)
        {
            var revInfo = await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = new string[] {"--no-sandbox", "--disable-setuid-sandbox"},
//                ExecutablePath = "/usr/bin/chromium-browser"
//                ExecutablePath = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe"
            });
            var page = await browser.NewPageAsync();
            await page.GoToAsync(parameters.bodyUrl);
            var file = Path.GetTempFileName();
            await page.PdfAsync(file);
            var closePageTask = page.CloseAsync();
            
            var content = await System.IO.File.ReadAllBytesAsync(file);
            var stream = new MemoryStream(content);
            System.IO.File.Delete(file);

            await closePageTask;
            
            return new FileStreamResult(stream, "application/pdf") {FileDownloadName = "document.pdf"};
        }
    }
}