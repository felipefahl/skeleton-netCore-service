using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skeleton.ServiceName.Business.Interfaces;
using Skeleton.ServiceName.Data.Models;
using Skeleton.ServiceName.ViewModel.People;

namespace Skeleton.ServiceName.API.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    public class PeopleController : CrudController<Person, PersonViewModel>
    {
        private new readonly IPersonService _service;

        public PeopleController(IPersonService personService)
            :base(personService)
        {
            _service = personService;
        }
    }
}