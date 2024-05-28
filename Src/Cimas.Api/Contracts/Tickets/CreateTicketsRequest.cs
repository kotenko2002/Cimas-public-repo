using Cimas.Domain.Entities.Tickets;

namespace Cimas.Api.Contracts.Tickets
{
    public record CreateTicketsRequest(
        List<CreateTicketRequestModel> Tickets
    );

    public record CreateTicketRequestModel(
        Guid SeatId,
        TicketStatus Status
    );
}
