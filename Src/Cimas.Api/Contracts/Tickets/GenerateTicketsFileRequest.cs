namespace Cimas.Api.Contracts.Tickets
{
    public record GenerateTicketsFileRequest(
        List<Guid> IdsOfSoldTickets
    );
}
