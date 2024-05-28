using Cimas.Domain.Entities.Tickets;

namespace Cimas.Api.Contracts.Tickets
{
    public record UpdateTicketsRequest(
        List<UpdateTicketRequestModel> Tickets
    );

    public record UpdateTicketRequestModel(
        Guid TicketId,
        TicketStatus Status
    );
}
