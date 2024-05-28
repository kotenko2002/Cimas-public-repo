using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Cinemas;
using Cimas.Domain.Entities.Halls;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Halls.Commands.CreateHall
{
    public class CreateHallCommandHandler : IRequestHandler<CreateHallCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _uow;

        public CreateHallCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<Success>> Handle(CreateHallCommand command, CancellationToken cancellationToken)
        {
            Cinema cinema = await _uow.CinemaRepository.GetByIdAsync(command.CinemaId);
            if (cinema is null)
            {
                return Error.NotFound(description: "Cinema with such id does not exist");
            }

            User user = await _uow.UserRepository.GetByIdAsync(command.UserId);
            if (user.CompanyId != cinema.CompanyId)
            {
                return Error.Forbidden(description: "You do not have the necessary permissions to perform this action");
            }

            var hall = new Hall()
            {
                CinemaId = command.CinemaId,
                Name = command.Name
            };
            await _uow.HallRepository.AddAsync(hall);

            List<HallSeat> seats = GenerateSeats(hall, command.NumberOfRows, command.NumberOfColumns);
            await _uow.SeatRepository.AddRangeAsync(seats);

            await _uow.CompleteAsync();

            return Result.Success;
        }

        private List<HallSeat> GenerateSeats(Hall hall, int numberOfRows, int numberOfColumns)
        {
            List<HallSeat> seats = new();

            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    seats.Add(new HallSeat()
                    {
                        Row = i,
                        Column = j,
                        Hall = hall,
                        Status = HallSeatStatus.Available
                    });
                }
            }

            return seats;
        }
    }
}
