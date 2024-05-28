using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Halls;
using Cimas.Domain.Entities.Sessions;
using Cimas.Domain.Entities.Tickets;
using Cimas.Domain.Entities.Users;
using Cimas.Domain.Models.Sessions;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Sessions.Queries.GetSeatsBySessionId
{
    public class GetSeatsBySessionIdHandler : IRequestHandler<GetSeatsBySessionIdQuery, ErrorOr<List<SessionSeat>>>
    {
        private readonly IUnitOfWork _uow;

        public GetSeatsBySessionIdHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<List<SessionSeat>>> Handle(GetSeatsBySessionIdQuery query, CancellationToken cancellationToken)
        {
            Session session = await _uow.SessionRepository.GetSessionIncludedTicketsByIdAsync(query.SessionId);
            if (session is null)
            {
                return Error.NotFound(description: "Session with such id does not exist");
            }

            Hall hall = await _uow.HallRepository.GetHallIncludedCinemaAndSeatsByIdAsync(session.HallId);
            User user = await _uow.UserRepository.GetByIdAsync(query.UserId);
            if (user.CompanyId != hall.Cinema.CompanyId)
            {
                return Error.Forbidden(description: "You do not have the necessary permissions to perform this action");
            }

            Dictionary<Guid, Ticket> ticketsDict = session.Tickets.ToDictionary(ticket => ticket.SeatId, ticket => ticket);

            List<SessionSeat> sessionSeats = hall.Seats.Select(hallSeat =>
            {
                ticketsDict.TryGetValue(hallSeat.Id, out var ticket);

                return new SessionSeat
                {
                    TicketId = ticket?.Id,
                    SeatId = hallSeat.Id,
                    Row = hallSeat.Row,
                    Column = hallSeat.Column,
                    Status = ticket != null
                        ? (SessionSeatStatus)ticket.Status
                        : (SessionSeatStatus)hallSeat.Status
                };
            }).ToList();

            return sessionSeats;
        }
    }
}
