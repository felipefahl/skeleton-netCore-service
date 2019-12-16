using System;
using System.Collections.Generic;
using System.Text;

namespace Skeleton.ServiceName.Data.Models
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }

        public DateTime? DateCreated { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public string UserModified { get; set; }
    }
}
