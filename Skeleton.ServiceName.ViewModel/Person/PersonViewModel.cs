using Skeleton.ServiceName.Utils.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Skeleton.ServiceName.ViewModel.People
{
    public class PersonViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Global))]
        [Display(Name = "FirstName", ResourceType = typeof(Global))]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }
    }

}
