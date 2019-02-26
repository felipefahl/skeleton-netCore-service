using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skeleton.ServiceName.Business.Interfaces;
using Skeleton.ServiceName.Utils.Exceptions;
using Skeleton.ServiceName.Utils.Models;
using Skeleton.ServiceName.ViewModel.People;
using System;
using System.Threading.Tasks;

namespace Skeleton.ServiceName.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PeopleController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public IActionResult ListAll()
        {
            var list = _personService.All();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var model = await _personService.GetAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] PersonViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newModel = await _personService.SaveAsync(model);
                var uri = $"Get/{newModel.Id}";
                return Created(uri, newModel); //201
            }
            throw new HttpStatusCodeException(StatusCodes.Status400BadRequest, ErrorResponse.FromModelStateError(ModelState));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PersonViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newModel = await _personService.SaveAsync(model);
                return Ok(newModel); //200
            }
            throw new HttpStatusCodeException(StatusCodes.Status400BadRequest, ErrorResponse.FromModelStateError(ModelState));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _personService.DeleteAsync(id))
            {
                return NotFound();
            }
            return NoContent(); //203
        }
    }
}