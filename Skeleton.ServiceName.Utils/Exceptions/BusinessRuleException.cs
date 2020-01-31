using Newtonsoft.Json.Linq;
using Skeleton.ServiceName.Utils.Models;
using System;

namespace Skeleton.ServiceName.Utils.Exceptions
{
    public class BusinessRuleException : Exception
    {
        public string ContentType { get; set; } = @"text/plain";

        public BusinessRuleException(string message) : base(message)
        {
        }

        public BusinessRuleException(Exception inner) : this(inner.ToString()) { }

        public BusinessRuleException(ErrorResponse errorResponse) : this(JObject.FromObject(errorResponse)) { }

        public BusinessRuleException(JObject errorObject) : this(errorObject.ToString())
        {
            this.ContentType = @"application/json";
        }
    }
}
