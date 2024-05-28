using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Users.Commands.FireUser
{
    public class FireUserHandler : IRequestHandler<FireUserCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _uow;

        public FireUserHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<Success>> Handle(FireUserCommand command, CancellationToken cancellationToken)
        {
            User owner = await _uow.UserRepository.GetByIdAsync(command.OwnerUserId);
            User userToFire = await _uow.UserRepository.GetByIdAsync(command.UserToFireId);

            if (userToFire is null)
            {
                return Error.NotFound(description: "User with such id does not exist");
            }

            if (owner.CompanyId != userToFire.CompanyId)
            {
                return Error.Forbidden(description: "You do not have the necessary permissions to perform this action");
            }

            userToFire.IsFired = true;

            await _uow.CompleteAsync();

            return Result.Success;
        }
    }
}
