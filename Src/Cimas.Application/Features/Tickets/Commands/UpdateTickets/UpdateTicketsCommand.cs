using Cimas.Domain.Entities.Tickets;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Tickets.Commands.UpdateTickets
{
    public record UpdateTicketsCommand(
        Guid UserId,
        List<UpdateTicketModel> Tickets
    ) : IRequest<ErrorOr<List<Guid>>>;

    public record UpdateTicketModel(
       Guid TicketId,
       TicketStatus Status
    );
}
