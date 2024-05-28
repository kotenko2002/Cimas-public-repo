using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Cinemas;
using Cimas.Domain.Entities.Films;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Films.Commands.CreateFilm
{
    public class CreateFilmHandler : IRequestHandler<CreateFilmCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _uow;

        public CreateFilmHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<Success>> Handle(CreateFilmCommand command, CancellationToken cancellationToken)
        {
            Cinema cinema = await _uow.CinemaRepository.GetByIdAsync(command.CinemaId);
            if (cinema is null)
            {
                return Error.NotFound(description: "Cinema with such id does not exist");
            }

            User user = await _uow.UserRepository.GetByIdAsync(command.UserId);
            if (user.CompanyId != cinema.CompanyId)
            {
                return Error.Forbidden(description: "You do not have the necessary permissions to perform this action");
            }

            var film = new Film()
            {
                CinemaId = command.CinemaId,
                Name = command.Name,
                Duration = command.Duration,
                IsDeleted = false
            };
            await _uow.FilmRepository.AddAsync(film);

            await _uow.CompleteAsync();

            return Result.Success;
        }
    }
}
