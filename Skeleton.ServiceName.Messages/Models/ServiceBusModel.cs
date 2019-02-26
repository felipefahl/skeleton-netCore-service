using Skeleton.ServiceName.Utils.Enumerators;
using System;
using System.Collections.Generic;

namespace Skeleton.ServiceName.Messages.Models
{
    public class ServiceBusModel
    {
        public DateTime Date { get; set; }

        public IList<string> Stack { get; set; }

        public string Uri { get; set; }

        public string UriReturn { get; set; }

        public ERestOperation ERestOperation { get; set; }

        public ERestOperation? ERestOperationReturn { get; set; }

        public object Obj { get; set; }
    }
}
