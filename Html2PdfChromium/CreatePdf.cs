using System;
using System.IO;
using System.Threading.Tasks;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace Html2PdfChromium
{
    public class CreatePdf
    {
        public static async Task<Stream> CreatePdfSync(PdfParameters parameters)
        {
            var guid = Guid.NewGuid().ToString();
            Console.WriteLine($"{guid} {DateTime.Now}: PDF generation for {parameters.BodyUrl} starts");
            var revInfo = await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            
            var launchOptions = new LaunchOptions
            {
                Headless = true,
                Args = new string[] {"--no-sandbox", "--disable-setuid-sandbox"},
            };
            using (var browser = await Puppeteer.LaunchAsync(launchOptions))
            {
                Console.WriteLine($"{guid} {DateTime.Now}: Browser loaded");
                Stream pdf;
                using (var page = await browser.NewPageAsync())
                {
                    var ops = new NavigationOptions()
                    {
                        Timeout = parameters.TimeOut,
                        WaitUntil = new [] {WaitUntilNavigation.Networkidle0, WaitUntilNavigation.DOMContentLoaded, WaitUntilNavigation.Load}
                    };
                    var response = await page.GoToAsync(parameters.BodyUrl, ops);
                    Console.WriteLine($"{guid} {DateTime.Now}: Page loaded");

                    if (!response.Ok)
                    {
                        var message = $"{guid} {DateTime.Now}: Page returned with an error {response.Status}";
                        Console.WriteLine(message);
                        throw new Exception(message);
                    }

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

                    pdf = await page.PdfStreamAsync(pdfOptions);
                    Console.WriteLine($"{guid} {DateTime.Now}: PDF generated");
                
                    var closePageTask = page.CloseAsync();
                    await closePageTask;
                    Console.WriteLine($"{guid} {DateTime.Now}: page closed");
                }

                var closeBrowserTask = browser.CloseAsync();
                await closeBrowserTask;
                Console.WriteLine($"{guid} {DateTime.Now}: browser closed");
                
                return pdf;
            }
        }

        private static PaperFormat ConvertToPaperFormat(string paperFormat)
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