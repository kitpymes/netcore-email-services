using Microsoft.Exchange.WebServices.Data;

namespace Email
{
    public class EmailExchangeModel
    {
        public EmailExchangeModel(ExchangeVersion exchangeVersion, TraceFlags traceFlags)
        {
            ExchangeVersion = exchangeVersion;
            TraceFlags = traceFlags;
        }

        public ExchangeVersion ExchangeVersion { get; }
        public TraceFlags TraceFlags { get; }
        public bool IsTraceEnabled { get { return !string.IsNullOrEmpty(TraceFlags.ToString()); } }
    }
}
