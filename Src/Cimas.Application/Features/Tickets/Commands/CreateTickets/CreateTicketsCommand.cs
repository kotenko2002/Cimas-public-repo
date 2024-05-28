using Cimas.Domain.Entities.Tickets;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Tickets.Commands.CreateTickets
{
    public record CreateTicketsCommand(
        Guid UserId,
        Guid SessionId,
        List<CreateTicketModel> Tickets
    ) : IRequest<ErrorOr<List<Guid>>>;

    public record CreateTicketModel(
       Guid SeatId,
       TicketStatus Status
    );
}
