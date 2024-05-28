using Cimas.Application.Common.Extensions;
using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Sessions;
using Cimas.Domain.Entities.Tickets;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Tickets.Commands.UpdateTickets
{
    public class UpdateTicketsHandler : IRequestHandler<UpdateTicketsCommand, ErrorOr<List<Guid>>>
    {
        private readonly IUnitOfWork _uow;

        public UpdateTicketsHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<List<Guid>>> Handle(UpdateTicketsCommand command, CancellationToken cancellationToken)
        {
            Dictionary<Guid, TicketStatus> ticketsToUpdate = command.Tickets.ToDictionary(t => t.TicketId, t => t.Status);
            List<Guid> ticketIds = ticketsToUpdate.Keys.ToList();
            List<Ticket> tickets = await _uow.TicketRepository.GetTicketsByIdsAsync(ticketIds);

            if (tickets.Count != ticketIds.Count)
            {
                return Error.NotFound(description: "One or more tickets with such ids does not exist");
            }

            Guid? sessionId = tickets.GetSingleDistinctIdOrNull(ticket => ticket.SessionId);
            if (!sessionId.HasValue)
            {
                return Error.Failure(description: "Session Ids are not the same in all tickets");
            }

            Session session = await _uow.SessionRepository.GetSessionsIncludedHallThenIncludedCinemaByIdAsync(sessionId.Value);
            User user = await _uow.UserRepository.GetByIdAsync(command.UserId);
            if (user.CompanyId != session.Hall.Cinema.CompanyId)
            {
                return Error.Forbidden(description: "You do not have the necessary permissions to perform this action");
            }

            foreach (var ticket in tickets)
            {
                ticket.Status = ticketsToUpdate[ticket.Id];
            }

            await _uow.CompleteAsync();

            List<Guid> IdsOfSoldTickets = tickets
              .Where(t => t.Status == TicketStatus.Sold)
              .Select(t => t.Id)
              .ToList();

            return IdsOfSoldTickets;
        }
    }
}
