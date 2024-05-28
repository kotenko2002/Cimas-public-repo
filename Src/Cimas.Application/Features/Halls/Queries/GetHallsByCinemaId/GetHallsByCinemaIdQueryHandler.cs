using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Cinemas;
using Cimas.Domain.Entities.Halls;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Halls.Queries.GetHallsByCinemaId
{
    public class GetHallsByCinemaIdQueryHandler : IRequestHandler<GetHallsByCinemaIdQuery, ErrorOr<List<Hall>>>
    {
        private readonly IUnitOfWork _uow;

        public GetHallsByCinemaIdQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<List<Hall>>> Handle(GetHallsByCinemaIdQuery query, CancellationToken cancellationToken)
        {
            Cinema cinema = await _uow.CinemaRepository.GetByIdAsync(query.CinemaId);
            if (cinema is null)
            {
                return Error.NotFound(description: "Cinema with such id does not exist");
            }

            User user = await _uow.UserRepository.GetByIdAsync(query.UserId);
            if (user.CompanyId != cinema.CompanyId)
            {
                return Error.Forbidden(description: "You do not have the necessary permissions to perform this action");
            }

            return await _uow.HallRepository.GetHallsByCinemaIdIncludedSeatsAsync(query.CinemaId);
        }
    }
}
