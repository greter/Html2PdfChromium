using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Html2PdfChromium
{
    [Route("api/[controller]")]
    [ApiController]
    public class Html2PdfAsyncController : Controller
    {
        private static readonly Dictionary<string, ConversionJob> _jobs = new Dictionary<string, ConversionJob>();
        private static Dictionary<string, Stream> _pdfs = new Dictionary<string, Stream>();

        // POST api/Html2PdfAsync/{conversionId}
        [HttpPost("{conversionId}")]
        public async Task<IActionResult> GetResult(string conversionId)
        {
            // if job is in status Processing wait for status change for one minute 
            var requestStart = DateTime.Now;
            while (requestStart > DateTime.Now - TimeSpan.FromMinutes(1))
            {
                lock (_jobs)
                lock (_pdfs)
                {
                    if (!_jobs.ContainsKey(conversionId))
                    {
                        return NotFound();
                    }

                    switch (_jobs[conversionId].status)
                    {
                        case ConversionJob.Status.Processing:
                            break;
                        case ConversionJob.Status.Error:
                            _jobs.Remove(conversionId);
                            _pdfs.Remove(conversionId);
                            return StatusCode(StatusCodes.Status500InternalServerError);
                        case ConversionJob.Status.Done:
                            if (_pdfs.ContainsKey(conversionId))
                            {
                                var fileStreamResult = new FileStreamResult(_pdfs[conversionId], "application/pdf")
                                    {FileDownloadName = "document.pdf"};
                                _jobs.Remove(conversionId);
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
            lock (_jobs)
            {
                _jobs.Add(converionId, conversionJob);
            }

            return conversionJob;
        }

        public static async Task CreatePdfImpl(string conversionId, PdfParameters parameters)
        {
            try
            {
                var pdf = await CreatePdf.CreatePdfSync(parameters);
                lock (_jobs)
                lock (_pdfs)
                {
                    _jobs[conversionId].status = ConversionJob.Status.Done;
                    _pdfs.Add(conversionId, pdf);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{conversionId}: Exception in CreatePdf.CreatePdfSync:");
                Console.WriteLine(e);
                lock (_jobs)
                {
                    if (_jobs.ContainsKey(conversionId))
                    {
                        _jobs[conversionId].status = ConversionJob.Status.Error;
                    }
                }
            }
        }
    }
}