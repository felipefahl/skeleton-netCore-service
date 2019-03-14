using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skeleton.ServiceName.Business.Interfaces;
using Skeleton.ServiceName.Utils.Exceptions;
using Skeleton.ServiceName.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skeleton.ServiceName.API.Controllers
{
    public abstract class CrudController<TEntity, TEntityViewModel> : ApiBaseController where TEntity : class
                                                                                        where TEntityViewModel : class
    {
        protected readonly IServiceCrud<TEntity, TEntityViewModel> _service;

        protected CrudController(IServiceCrud<TEntity, TEntityViewModel> service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult ListAll()
        {
            var list = _service.All();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
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
            if (ModelState.IsValid)
            {
                var newModel = await _service.InsertAsync(model);
                return Ok(newModel); //200
                //TODO: Verificar uma forma de retornar a URL Created com a key do item inserido
                //var uri = $"Get/{newModel.Id}";
                //return Created(uri, newModel); //201
            }
            throw new HttpStatusCodeException(StatusCodes.Status400BadRequest, ErrorResponse.FromModelStateError(ModelState));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TEntityViewModel model)
        {
            if (ModelState.IsValid)
            {
                var updatedModel = await _service.UpdateAsync(model);
                return Ok(updatedModel); //200
            }
            throw new HttpStatusCodeException(StatusCodes.Status400BadRequest, ErrorResponse.FromModelStateError(ModelState));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _service.DeleteAsync(id))
            {
                return NotFound();
            }
            return NoContent(); //203
        }
    }
}
