using Skeleton.ServiceName.Utils.Resources;
using Skeleton.ServiceName.ViewModel.Base;
using System.ComponentModel.DataAnnotations;

namespace Skeleton.ServiceName.ViewModel.User
{
    public class UserViewModel : BaseViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Global))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Global))]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(Global))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Global))]
        [DataType(DataType.Password, ErrorMessageResourceName = "IsInvalid", ErrorMessageResourceType = typeof(Global))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Global))]
        [DataType(DataType.Password, ErrorMessageResourceName = "IsInvalid", ErrorMessageResourceType = typeof(Global))]
        [Compare("Password", ErrorMessageResourceName = "PasswordDoesNotMatch", ErrorMessageResourceType = typeof(Global))]
        public string PasswordCheck { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Global))]
        public string Profile { get; set; }
    }
}
