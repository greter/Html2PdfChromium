using System;

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
        
        public readonly string conversionId;
        public DateTime startTimeStamp;
        public Status status;

        public ConversionJob(string conversionId)
        {
            this.conversionId = conversionId;
            startTimeStamp = DateTime.Now;
            status = Status.Processing;
        }
    }
}
