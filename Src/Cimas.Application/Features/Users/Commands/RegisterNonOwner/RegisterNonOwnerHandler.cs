using Cimas.Application.Features.Auth.Commands.Register;
using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using Mapster;
using MediatR;

namespace Cimas.Application.Features.Users.Commands.RegisterNonOwner
{
    public class RegisterNonOwnerHandler : IRequestHandler<RegisterNonOwnerCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;

        public RegisterNonOwnerHandler(
            IUnitOfWork uow, IMediator mediator)
        {
            _uow = uow;
            _mediator = mediator;
        }

        public async Task<ErrorOr<Success>> Handle(RegisterNonOwnerCommand command, CancellationToken cancellationToken)
        {
            User owner = await _uow.UserRepository.GetByIdAsync(command.OwnerUserId);

            var registerCommand = (owner.CompanyId, Roles.Owner, command).Adapt<RegisterCommand>();

            return await _mediator.Send(registerCommand);
        }
    }
}
