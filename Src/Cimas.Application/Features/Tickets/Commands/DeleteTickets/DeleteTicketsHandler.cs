using Cimas.Application.Common.Extensions;
using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Sessions;
using Cimas.Domain.Entities.Tickets;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Tickets.Commands.DeleteTickets
{
    public class DeleteTicketsHandler : IRequestHandler<DeleteTicketsCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _uow;

        public DeleteTicketsHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<Success>> Handle(DeleteTicketsCommand command, CancellationToken cancellationToken)
        {
            List<Ticket> tickets = await _uow.TicketRepository.GetTicketsByIdsAsync(command.TicketIds);
            if (tickets.Count != command.TicketIds.Count)
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

            await _uow.TicketRepository.RemoveRangeAsync(tickets);

            await _uow.CompleteAsync();

            return Result.Success;
        }
    }
}
