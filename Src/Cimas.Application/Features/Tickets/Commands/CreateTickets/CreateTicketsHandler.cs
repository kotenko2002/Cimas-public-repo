using Cimas.Application.Common.Extensions;
using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Halls;
using Cimas.Domain.Entities.Sessions;
using Cimas.Domain.Entities.Tickets;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Tickets.Commands.CreateTickets
{
    public class CreateTicketsHandler : IRequestHandler<CreateTicketsCommand, ErrorOr<List<Guid>>>
    {
        private readonly IUnitOfWork _uow;

        public CreateTicketsHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<List<Guid>>> Handle(CreateTicketsCommand command, CancellationToken cancellationToken)
        {
            
            Session session = await _uow.SessionRepository.GetByIdAsync(command.SessionId);
            if (session is null)
            {
                return Error.NotFound(description: "Session with such id does not exist");
            }

            List<Guid> seatIds = command.Tickets.Select(ticket => ticket.SeatId).ToList();
            List<HallSeat> seats = await _uow.SeatRepository.GetSeatsByIdsAsync(seatIds);
            if (seatIds.Count != seats.Count)
            {
                return Error.NotFound(description: "One or more seats with such ids does not exist");
            }

            if(seats.Any(seat => seat.Status != HallSeatStatus.Available))
            {
                return Error.Failure(description: "One or more seats with such ids are not avalible");
            }

            Guid? hallId = seats.GetSingleDistinctIdOrNull(seat => seat.HallId);
            if (!hallId.HasValue || session.HallId != hallId.Value)
            {
                return Error.Failure(description: "The seat does not belong to the same hall as the session");
            }

            User user = await _uow.UserRepository.GetByIdAsync(command.UserId);
            Hall hall = await _uow.HallRepository.GetHallIncludedCinemaByIdAsync(hallId.Value);
            if (user.CompanyId != hall.Cinema.CompanyId)
            {
                return Error.Forbidden(description: "You do not have the necessary permissions to perform this action");
            }

            bool isTicketsAlreadyExists = await _uow.TicketRepository.TicketsAlreadyExistsAsync(
                session.Id,
                seatIds);
            if (isTicketsAlreadyExists)
            {
                return Error.Failure(description: "Ticket for one of the seats is already sold out");
            }

            List<Ticket> tickets = command.Tickets
                .Select(ticket => new Ticket()
                {
                    Session = session,
                    Seat = seats.First(seat => seat.Id == ticket.SeatId),
                    CreationTime = DateTime.UtcNow,
                    Status = ticket.Status
                }).ToList();

            await _uow.TicketRepository.AddRangeAsync(tickets);
            await _uow.CompleteAsync();

            List<Guid> IdsOfSoldTickets = tickets
                .Where(t => t.Status == TicketStatus.Sold)
                .Select(t => t.Id)
                .ToList();

            return IdsOfSoldTickets;
        }
    }
}
