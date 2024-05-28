using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Cinemas;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Cinemas.Queries.GetAllCinemas
{
    public class GetAllCinemasQueryHandler : IRequestHandler<GetAllCinemasQuery, ErrorOr<List<Cinema>>>
    {
        private readonly IUnitOfWork _uow;

        public GetAllCinemasQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<List<Cinema>>> Handle(GetAllCinemasQuery query, CancellationToken cancellationToken)
        {
            User user = await _uow.UserRepository.GetByIdAsync(query.UserId);

            return await _uow.CinemaRepository.GetCinemasByCompanyIdAsync(user.CompanyId);
        }
    }
}
