using Cimas.Domain.Entities.WorkDays;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Workdays.Commands.StartWorkday
{
    public record StartWorkdayCommand(
        Guid UserId,
        Guid CinemaId
    ) : IRequest<ErrorOr<Workday>>;
}
