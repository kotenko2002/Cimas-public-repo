using Cimas.Application.Features.Auth.Commands.Register;
using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Companies;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Cimas.Application.Features.Auth.Commands.RegisterOwner
{
    public class RegisterOwnerHandler : IRequestHandler<RegisterOwnerCommand, ErrorOr<Success>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IPasswordValidator<User> _passwordValidator;
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;
        
        public RegisterOwnerHandler(
            UserManager<User> userManager,
            IPasswordValidator<User> passwordValidator,
            IUnitOfWork uow,
            IMediator mediator)
        {
            _userManager = userManager;
            _passwordValidator = passwordValidator;
            _uow = uow;
            _mediator = mediator;
        }

        public async Task<ErrorOr<Success>> Handle(RegisterOwnerCommand command, CancellationToken cancellationToken)
        {
            IdentityResult passwordValidationResult = await _passwordValidator.ValidateAsync(_userManager, null, command.Password);
            if (!passwordValidationResult.Succeeded)
            {
                return passwordValidationResult.Errors
                    .Select(e => Error.Validation(
                        code: "Password",
                        description: e.Description))
                    .ToList();
            }

            var company = new Company()
            {
                Id = Guid.NewGuid(),
                Name = command.CompanyName,
            };

            await _uow.CompanyRepository.AddAsync(company);
            await _uow.CompleteAsync();

            var registerCommand = (company.Id, Roles.Owner, command).Adapt<RegisterCommand>();
            ErrorOr<Success> registerCommandResult = await _mediator.Send(registerCommand);

            if (registerCommandResult.IsError)
            {
                await _uow.CompanyRepository.RemoveAsync(company);
                await _uow.CompleteAsync();

                return registerCommandResult.Errors;
            }

            return Result.Success;
        }
    }
}
