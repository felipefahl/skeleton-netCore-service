using Skeleton.ServiceName.Utils.Enumerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Skeleton.ServiceName.Messages.Models
{
    public class ApplicationInsightsChoreographyStackModel
    {
        public DateTime Date { get; set; }
        public IList<string> Stack { get; set; }

        public EChoreographyStackType Type { get; set; }

        public object Object { get; set; }

    }
}
