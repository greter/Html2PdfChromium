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
        public async Task<IActionResult> PolicyStatement([FromBody] PdfParameters parameters)
        {
            var revInfo = await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = new string[] {"--no-sandbox", "--disable-setuid-sandbox"},
            });
            var page = await browser.NewPageAsync();
            await page.GoToAsync(parameters.BodyUrl);
            var pdfOptions = new PdfOptions();
            pdfOptions.Scale = parameters.Scale;
            pdfOptions.DisplayHeaderFooter = parameters.DisplayHeaderFooter;
            pdfOptions.HeaderTemplate = parameters.HeaderTemplate;
            pdfOptions.FooterTemplate = parameters.FooterTemplate;
            pdfOptions.PrintBackground = parameters.PrintBackground;
            pdfOptions.Landscape = parameters.Landscape;
            pdfOptions.PageRanges = parameters.PageRanges;
            pdfOptions.Format = ConvertToPaperFormat(parameters.PaperFormat);
            pdfOptions.MarginOptions = new MarginOptions
            {
                Top = parameters.TopMargin,
                Left = parameters.LeftMargin,
                Bottom = parameters.BottomMargin,
                Right = parameters.RightMargin
            };
            pdfOptions.PreferCSSPageSize = parameters.PreferCSSPageSize;
            
            var pdf = await page.PdfStreamAsync(pdfOptions);
            var closePageTask = page.CloseAsync();
            
            await closePageTask;
            
            return new FileStreamResult(pdf, "application/pdf") {FileDownloadName = "document.pdf"};
        }

        private PaperFormat ConvertToPaperFormat(string paperFormat)
        {
            switch (paperFormat)
            {
                case null:
                    return null;
                case "Letter":
                    return PaperFormat.Letter;
                case "Legal":
                    return PaperFormat.Legal;
                case "Tabloid":
                    return PaperFormat.Tabloid;
                case "Ledger":
                    return PaperFormat.Ledger;
                case "A0":
                    return PaperFormat.A0;
                case "A1":
                    return PaperFormat.A1;
                case "A2":
                    return PaperFormat.A2;
                case "A3":
                    return PaperFormat.A3;
                case "A4":
                    return PaperFormat.A4;
                case "A5":
                    return PaperFormat.A5;
                case "A6":
                    return PaperFormat.A6;
                default:
                    return null;
            }
        }
    }
}