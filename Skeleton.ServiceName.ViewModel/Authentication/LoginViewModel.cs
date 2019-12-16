using Skeleton.ServiceName.Utils.Resources;
using System.ComponentModel.DataAnnotations;

namespace Skeleton.ServiceName.ViewModel.Authentication
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Global))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Global))]
        public string Password { get; set; }
    }
}
