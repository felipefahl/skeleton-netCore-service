using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skeleton.ServiceName.Business.Interfaces;
using Skeleton.ServiceName.Data.Models;
using Skeleton.ServiceName.ViewModel.User;

namespace Skeleton.ServiceName.API.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    public class UsersController : CrudController<User, UserViewModel>
    {
        private new readonly IUserService _service;
        public UsersController(IUserService service) : base(service)
        {
            _service = service;
        }
    }
}