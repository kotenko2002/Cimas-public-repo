using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Tickets.Commands.DeleteTickets
{
    public record DeleteTicketsCommand(
        Guid UserId,
        List<Guid> TicketIds
    ) : IRequest<ErrorOr<Success>>;
}
