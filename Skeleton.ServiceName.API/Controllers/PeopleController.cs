using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Skeleton.ServiceName.Business.Interfaces;
using Skeleton.ServiceName.Business.Parameters;
using Skeleton.ServiceName.Data.Models;
using Skeleton.ServiceName.Utils.Models;
using Skeleton.ServiceName.ViewModel.People;
using System.Threading.Tasks;

namespace Skeleton.ServiceName.API.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    public class PeopleController : CrudController<Person, PersonViewModel>
    {
        private new readonly IPersonService _service;

        public PeopleController(IPersonService personService)
            : base(personService)
        {
            _service = personService;
        }

        [HttpGet]
        public override async Task<IActionResult> ListAll([FromQuery] QueryStringParameters queryStringParameters)
        {
            var serializedParent = JsonConvert.SerializeObject(queryStringParameters);
            var personParameter = JsonConvert.DeserializeObject<PersonParameters>(serializedParent);

            personParameter.Name = HttpContext.Request.Query["Name"].ToString();

            var count = await _service.CountAsync(personParameter);
            var list = _service.All(personParameter);

            var metadata = new MetadataPagination(count, queryStringParameters.PageNumber, queryStringParameters.PageSize);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(list);
        }
    }
}