using System;

namespace Skeleton.ServiceName.ViewModel.Base
{
    public class BaseViewModel
    {
        public Guid? Id { get; set; }

        public bool Active { get; set; }

        public DateTime? DateCreated { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public string UserModified { get; set; }
    }
}
