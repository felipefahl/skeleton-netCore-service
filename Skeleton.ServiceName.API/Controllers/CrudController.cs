using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult ListAll()
        {
            try
            {
                var list = _service.All();
                return Ok(list);
            }
            catch (Exception e)
            {
                throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, ErrorResponse.From(e));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            try
            {
                var model = await _service.GetAsync(id);
                if (model == null)
                {
                    return NotFound();
                }
                return Ok(model);
            }
            catch (Exception e)
            {
                throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, ErrorResponse.From(e));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] TEntityViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newModel = await _service.InsertAsync(model);
                    var uri = $"Get/{newModel.Id}";
                    return Created(uri, newModel); //201
                }
                catch (Exception e)
                {
                    throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, ErrorResponse.From(e));
                }
            }
            throw new HttpStatusCodeException(StatusCodes.Status400BadRequest, ErrorResponse.FromModelStateError(ModelState));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TEntityViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var updatedModel = await _service.UpdateAsync(model);
                    return Ok(updatedModel); //200

                }
            }
            catch (Exception e)
            {
                throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, ErrorResponse.From(e));
            }
            throw new HttpStatusCodeException(StatusCodes.Status400BadRequest, ErrorResponse.FromModelStateError(ModelState));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (!await _service.DeleteAsync(id))
                {
                    return NotFound();
                }
                return NoContent(); //203
            }
            catch (Exception e)
            {
                throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, ErrorResponse.From(e));
            }
        }
    }
}
