using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Companies;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Cimas.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<Success>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _uow;

        public RegisterCommandHandler(
            UserManager<User> userManager,
            IUnitOfWork uow)
        {
            _userManager = userManager;
            _uow = uow;
        }

        public async Task<ErrorOr<Success>> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            User existsUser = await _userManager.FindByNameAsync(command.Username);
            if (existsUser is not null)
            {
                return Error.Conflict(description: "Обліковий запис з таким користувацьким іменем вже існує");
            }

            Company company = await _uow.CompanyRepository.GetByIdAsync(command.CompanyId);
            if (company is null)
            {
                return Error.NotFound(description: "Company with such id does not exist");
            }

            var user = new User()
            {
                Company = company,
                FirstName = command.FirstName,
                LastName = command.LastName,
                UserName = command.Username,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            IdentityResult result = await _userManager.CreateAsync(user, command.Password);
            if (!result.Succeeded)
            {
                return result.Errors
                    .Select(e => Error.Validation(
                        code: "Password",
                        description: e.Description))
                    .ToList();
            }

            await _userManager.AddToRoleAsync(user, command.Role);

            return Result.Success;
        }
    }
}
