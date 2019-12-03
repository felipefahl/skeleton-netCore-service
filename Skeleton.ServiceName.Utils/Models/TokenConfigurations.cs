using System.Collections.Generic;

namespace Skeleton.ServiceName.Utils.Models
{
    public class TokenConfigurations
    {
        public string Secret { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public long Seconds { get; set; }
    }
}
