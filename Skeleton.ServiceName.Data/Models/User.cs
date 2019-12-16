using System;
using System.Collections.Generic;
using System.Text;

namespace Skeleton.ServiceName.Data.Models
{
    public class User : BaseEntity
    {

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordCheck { get; set; }
        public string Profile { get; set; }
    }
}
