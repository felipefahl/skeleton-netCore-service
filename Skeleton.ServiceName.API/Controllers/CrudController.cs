using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Skeleton.ServiceName.Business.Interfaces;
using Skeleton.ServiceName.Utils.Exceptions;
using Skeleton.ServiceName.Utils.Models;
using Skeleton.ServiceName.ViewModel.Base;
using System;
using System.Threading.Tasks;

namespace Skeleton.ServiceName.API.Controllers
{
    [Produces("application/json")]
    public abstract class CrudController<TEntity, TEntityViewModel> : ApiBaseController where TEntity : class
                                                                                        where TEntityViewModel : BaseViewModel
    {
        protected readonly IServiceCrud<TEntity, TEntityViewModel> _service;

        protected CrudController(IServiceCrud<TEntity, TEntityViewModel> service)
        {
            _service = service;
        }

        [HttpGet]
        public async virtual Task<IActionResult> ListAll([FromQuery] QueryStringParameters queryStringParameters)
        {
            var count = await _service.CountAsync();
            var list = _service.All(queryStringParameters);

            var metadata = new MetadataPagination(count, queryStringParameters.PageNumber, queryStringParameters.PageSize);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var model = await _service.GetAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] TEntityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpStatusCodeException(StatusCodes.Status400BadRequest, ErrorResponse.FromModelStateError(ModelState));
            }

            var newModel = await _service.InsertAsync(model);
            var uri = $"Get/{newModel.Id}";
            return Created(uri, newModel); //201-
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TEntityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpStatusCodeException(StatusCodes.Status400BadRequest, ErrorResponse.FromModelStateError(ModelState));
            }
            var updatedModel = await _service.UpdateAsync(model);
            return Ok(updatedModel); //200
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _service.DeleteAsync(id))
            {
                return NotFound();
            }
            return NoContent(); //203
        }
    }
}
