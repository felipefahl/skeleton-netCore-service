using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skeleton.ServiceName.Business.Interfaces;
using Skeleton.ServiceName.Utils.Exceptions;
using Skeleton.ServiceName.Utils.Models;
using Skeleton.ServiceName.ViewModel.Authentication;

namespace Skeleton.ServiceName.API.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class AuthenticationsController : ApiBaseController
    {

        private readonly IUserService _userService;

        public AuthenticationsController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginUserAsync([FromBody]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newModel = await _userService.LoginAsync(model);

                    return Ok(newModel);

                }
                catch (Exception e)
                {
                    throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, ErrorResponse.From(e));
                }
            }

            throw new HttpStatusCodeException(StatusCodes.Status400BadRequest, ErrorResponse.FromModelStateError(ModelState));
        }
    }
}