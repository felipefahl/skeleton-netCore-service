using Skeleton.ServiceName.Utils.Resources;
using Skeleton.ServiceName.ViewModel.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Skeleton.ServiceName.ViewModel.People
{
    public class PersonViewModel : BaseViewModel
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }
    }

}
