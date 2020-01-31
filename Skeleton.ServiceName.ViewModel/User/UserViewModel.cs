using Skeleton.ServiceName.ViewModel.Base;

namespace Skeleton.ServiceName.ViewModel.User
{
    public class UserViewModel : BaseViewModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordCheck { get; set; }

        public string Profile { get; set; }
    }
}
