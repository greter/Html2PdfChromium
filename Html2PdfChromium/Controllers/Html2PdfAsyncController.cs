using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Html2PdfChromium.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Html2PdfAsyncController : Controller
    {
        private static readonly Dictionary<string, ConversionJob> Jobs = new Dictionary<string, ConversionJob>();
        private static Dictionary<string, Stream> _pdfs = new Dictionary<string, Stream>();

        // POST api/Html2PdfAsync/{conversionId}
        [HttpPost("{conversionId}")]
        public async Task<IActionResult> GetResult(string conversionId)
        {
            // if job is in status Processing wait for status change for one minute 
            var requestStart = DateTime.Now;
            while (requestStart > DateTime.Now - TimeSpan.FromMinutes(1))
            {
                lock (Jobs)
                lock (_pdfs)
                {
                    if (!Jobs.ContainsKey(conversionId))
                    {
                        return NotFound();
                    }

                    switch (Jobs[conversionId].TheStatus)
                    {
                        case ConversionJob.Status.Processing:
                            break;
                        case ConversionJob.Status.Error:
                            Jobs.Remove(conversionId);
                            _pdfs.Remove(conversionId);
                            return StatusCode(StatusCodes.Status500InternalServerError);
                        case ConversionJob.Status.Done:
                            if (_pdfs.ContainsKey(conversionId))
                            {
                                var fileStreamResult = new FileStreamResult(_pdfs[conversionId], "application/pdf")
                                    {FileDownloadName = "document.pdf"};
                                Jobs.Remove(conversionId);
                                _pdfs.Remove(conversionId);
                                return fileStreamResult;
                            }
                            else
                            {
                                return StatusCode(StatusCodes.Status500InternalServerError);
                            }
                    }
                }
                await Task.Delay(100);
            }
            return Accepted();
        }

        //POST api/Html2PdfAsync
        [HttpPost]
        public ActionResult<ConversionJob> CreatePdfAsync([FromBody] PdfParameters parameters)
        {
            var converionId = Guid.NewGuid().ToString();
            BackgroundJob.Enqueue(() => CreatePdfImpl(converionId, parameters));

            var conversionJob = new ConversionJob(converionId);
            lock (Jobs)
            {
                Jobs.Add(converionId, conversionJob);
            }

            return conversionJob;
        }

        public static async Task CreatePdfImpl(string conversionId, PdfParameters parameters)
        {
            try
            {
                var pdf = await CreatePdf.CreatePdfSync(parameters);
                lock (Jobs)
                lock (_pdfs)
                {
                    Jobs[conversionId].TheStatus = ConversionJob.Status.Done;
                    _pdfs.Add(conversionId, pdf);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{conversionId}: Exception in CreatePdf.CreatePdfSync:");
                Console.WriteLine(e);
                lock (Jobs)
                {
                    if (Jobs.ContainsKey(conversionId))
                    {
                        Jobs[conversionId].TheStatus = ConversionJob.Status.Error;
                    }
                }
            }
        }
    }
}