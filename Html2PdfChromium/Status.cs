namespace Html2PdfChromium
{
    public class Status
    {
        public const string Ok = "Ok";
        public const string Error = "Error";

        public readonly string State;

        public Status(string state)
        {
            this.State = state;
        }
    }
}