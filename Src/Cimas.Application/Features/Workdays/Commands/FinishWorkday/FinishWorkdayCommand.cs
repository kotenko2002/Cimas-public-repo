using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Workdays.Commands.FinishWorkday
{
    public record FinishWorkdayCommand(
        Guid UserId
    ) : IRequest<ErrorOr<Success>>;
}
