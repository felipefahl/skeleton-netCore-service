using System;

namespace Skeleton.ServiceName.ViewModel.Authentication
{
    public class LoginResponseViewModel
    {
        public bool Success { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Profile { get; set; }

        public string Token { get; set; }

        public bool Active { get; set; }

        public string Message { get; set; }
    }
}
