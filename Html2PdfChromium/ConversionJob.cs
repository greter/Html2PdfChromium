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

        private readonly string conversionId;
        private DateTime startTimeStamp;
        public Status TheStatus;

        public ConversionJob(string conversionId)
        {
            this.conversionId = conversionId;
            startTimeStamp = DateTime.Now;
            TheStatus = Status.Processing;
        }
    }
}
