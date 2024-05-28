using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Cinemas;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Cinemas.Commands.CreateCinema
{
    public class CreateCinemaCommandHandler : IRequestHandler<CreateCinemaCommand, ErrorOr<Cinema>>
    {
        private readonly IUnitOfWork _uow;

        public CreateCinemaCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<Cinema>> Handle(CreateCinemaCommand command, CancellationToken cancellationToken)
        {
            User user = await _uow.UserRepository.GetByIdAsync(command.UserId);
     
            var cinema = new Cinema()
            {
                Id = Guid.NewGuid(),
                CompanyId = user.CompanyId,
                Name = command.Name,
                Address = command.Address
            };

            await _uow.CinemaRepository.AddAsync(cinema);
            await _uow.CompleteAsync();

            return cinema;
        }
    }
}
