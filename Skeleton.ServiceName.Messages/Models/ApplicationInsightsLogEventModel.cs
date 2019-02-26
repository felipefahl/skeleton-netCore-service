using Skeleton.ServiceName.Utils.Enumerators;
using System;

namespace Skeleton.ServiceName.Messages.Models
{
    public class ApplicationInsightsLogEventModel
    {
        public DateTime Date { get; set; }
        public ELogType Type { get; set; }

        public string Message { get; set; }

        public string Source { get; set; }
    }
}
