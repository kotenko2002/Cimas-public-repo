using Cimas.Domain.Entities.WorkDays;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Workdays.Queries.GetCurrentWorkday
{
    public record GetCurrentWorkdayQuery(
        Guid UserId
    ) : IRequest<ErrorOr<Workday>>;
}
