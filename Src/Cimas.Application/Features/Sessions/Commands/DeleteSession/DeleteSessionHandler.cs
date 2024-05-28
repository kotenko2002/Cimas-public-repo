using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Sessions;
using Cimas.Domain.Entities.Tickets;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Cimas.Application.Features.Sessions.Commands.DeleteSession
{
    public class DeleteSessionHandler : IRequestHandler<DeleteSessionCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _uow;

        public DeleteSessionHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<Success>> Handle(DeleteSessionCommand command, CancellationToken cancellationToken)
        {
            Session session = await _uow.SessionRepository.GetSessionsIncludedHallThenIncludedCinemaByIdAsync(command.SessionId);
            if (session is null)
            {
                return Error.NotFound(description: "Session with such id does not exist");
            }

            User user = await _uow.UserRepository.GetByIdAsync(command.UserId);
            if (user.CompanyId != session.Hall.Cinema.CompanyId)
            {
                return Error.Forbidden(description: "You do not have the necessary permissions to perform this action");
            }

            List<Ticket> tickets = await _uow.TicketRepository.GetTicketsBySessionIdAsync(session.Id);
            if (tickets.Any())
            {
                await _uow.TicketRepository.RemoveRangeAsync(tickets);
            }

            await _uow.SessionRepository.RemoveAsync(session);

            await _uow.CompleteAsync();

            return Result.Success;
        }
    }
}
