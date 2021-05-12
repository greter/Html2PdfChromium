using System;
using System.Threading;
using System.Threading.Tasks;

namespace Html2PdfChromium
{
    public class ConversionJob
    {
        public enum Status
        {
            Processing,
            Done,
            Error
        }

        public string conversionId  { get; }
        public DateTime startTimeStamp { get;  }
        public Status status  { get; set; }

        public Thread thread;

        public ConversionJob(string conversionId, Thread thread)
        {
            this.conversionId = conversionId;
            this.thread = thread;
            startTimeStamp = DateTime.Now;
            status = Status.Processing;
        }
    }
}
