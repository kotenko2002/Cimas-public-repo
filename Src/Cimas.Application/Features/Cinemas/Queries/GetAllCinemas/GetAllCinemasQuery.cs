using Cimas.Domain.Entities.Cinemas;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Cinemas.Queries.GetAllCinemas
{
    public record GetAllCinemasQuery(
        Guid UserId
    ) : IRequest<ErrorOr<List<Cinema>>>;
}
