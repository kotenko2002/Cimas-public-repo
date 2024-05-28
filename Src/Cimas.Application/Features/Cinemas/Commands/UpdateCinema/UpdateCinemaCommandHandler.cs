using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Cinemas;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Cinemas.Commands.UpdateCinema
{
    public class UpdateCinemaCommandHandler : IRequestHandler<UpdateCinemaCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _uow;

        public UpdateCinemaCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<Success>> Handle(UpdateCinemaCommand command, CancellationToken cancellationToken)
        {
            Cinema cinema = await _uow.CinemaRepository.GetByIdAsync(command.CinemaId);
            if(cinema is null)
            {
                return Error.NotFound(description: "Cinema with such id does not exist");
            }
            
            User user = await _uow.UserRepository.GetByIdAsync(command.UserId);
            if (user.CompanyId != cinema.CompanyId)
            {
                return Error.Forbidden(description: "You do not have the necessary permissions to perform this action");
            }

            cinema.Name = command.Name;
            cinema.Address = command.Address;

            await _uow.CompleteAsync();

            return Result.Success;
        }
    }
}
