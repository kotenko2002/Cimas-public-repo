using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Halls;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Halls.Commands.UpdateHallSeats
{
    public class UpdateHallSeatsCommandHandler : IRequestHandler<UpdateHallSeatsCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _uow;

        public UpdateHallSeatsCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<Success>> Handle(UpdateHallSeatsCommand command, CancellationToken cancellationToken)
        {
            Hall hall = await _uow.HallRepository.GetHallIncludedCinemaByIdAsync(command.HallId);
            if (hall is null)
            {
                return Error.NotFound(description: "Hall with such id does not exist");
            }

            User user = await _uow.UserRepository.GetByIdAsync(command.UserId);
            if (user.CompanyId != hall.Cinema.CompanyId)
            {
                return Error.Forbidden(description: "You do not have the necessary permissions to perform this action");
            }

            List<HallSeat> seats = await _uow.SeatRepository.GetSeatsByIdsAsync(command.Seats.Select(seat => seat.Id));

            UpdateSeat invalidSeat = command.Seats
                .FirstOrDefault(commandSeat => !seats.Any(s => s.Id == commandSeat.Id));
            if (invalidSeat != null)
            {
                return Error.NotFound(description: $"Seat with id '{invalidSeat.Id}' does not exist");
            }

            foreach (UpdateSeat commandSeat in command.Seats)
            {
                HallSeat seat = seats.First(s => s.Id == commandSeat.Id);
                seat.Status = commandSeat.Status;
            }

            // TODO: update numbers of seats

            await _uow.CompleteAsync();

            return Result.Success;
        }
    }
}
