using Cimas.Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cimas.Api.Controllers
{
    [Route("dev")]
    [ApiController]
    public class aCostilDevController : BaseController
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public aCostilDevController(
            IMediator mediator,
            RoleManager<IdentityRole<Guid>> roleManager
        ) : base(mediator)
        {
            _roleManager = roleManager;
        }

        [HttpPost("AddRolesToDb")]
        public async Task AddRolesToDb() 
        {
            foreach (var role in Roles.GetRoles())
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }
        }
    }
}
