using Skeleton.ServiceName.Utils.Resources;
using Skeleton.ServiceName.ViewModel.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Skeleton.ServiceName.ViewModel.People
{
    public class PersonViewModel : BaseViewModel
    {

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Global))]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Global))]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Global))]
        public DateTime BirthDate { get; set; }
    }

}
