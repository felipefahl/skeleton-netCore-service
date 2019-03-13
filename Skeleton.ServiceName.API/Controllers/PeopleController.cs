using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skeleton.ServiceName.Business.Interfaces;
using Skeleton.ServiceName.Data;
using Skeleton.ServiceName.Utils.Exceptions;
using Skeleton.ServiceName.Utils.Models;
using Skeleton.ServiceName.ViewModel.People;
using System;
using System.Threading.Tasks;

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