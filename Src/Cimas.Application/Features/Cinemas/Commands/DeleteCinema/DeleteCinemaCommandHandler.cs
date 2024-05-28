using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Cinemas;
using Cimas.Domain.Entities.Films;
using Cimas.Domain.Entities.Halls;
using Cimas.Domain.Entities.Products;
using Cimas.Domain.Entities.Reports;
using Cimas.Domain.Entities.Sessions;
using Cimas.Domain.Entities.Tickets;
using Cimas.Domain.Entities.Users;
using Cimas.Domain.Entities.WorkDays;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Cinemas.Commands.DeleteCinema
{
    public class DeleteCinemaCommandHandler : IRequestHandler<DeleteCinemaCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _uow;

        public DeleteCinemaCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<Success>> Handle(DeleteCinemaCommand command, CancellationToken cancellationToken)
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

            await DeleteCinemaAndAllDependentData(cinema.Id);

            await _uow.CompleteAsync();

            return Result.Success;
        }

        private async Task DeleteCinemaAndAllDependentData(Guid cinemaId)
        {
            Cinema cinema = await _uow.CinemaRepository.GetCinemaIncludedAllDependentDataByIdAsync(cinemaId);

            List<Product> products = cinema.Products.ToList();
            if (products.Any())
            {
                await _uow.ProductRepository.RemoveRangeAsync(products);
            }

            List<Ticket> tickets = cinema.Halls
                .SelectMany(hall => hall.Seats
                    .SelectMany(seat => seat.Tickets)
                ).ToList();
            if (tickets.Any())
            {
                await _uow.TicketRepository.RemoveRangeAsync(tickets);
            }

            List<HallSeat> hallSeats = cinema.Halls
                .SelectMany(hall => hall.Seats)
                .ToList();
            if (hallSeats.Any())
            {
                await _uow.SeatRepository.RemoveRangeAsync(hallSeats);
            }

            List<Session> sessions = cinema.Films
                .SelectMany(film => film.Sessions)
                .ToList();
            if (sessions.Any())
            {
                await _uow.SessionRepository.RemoveRangeAsync(sessions);
            }

            List<Hall> halls = cinema.Halls.ToList();
            if (halls.Any())
            {
                 await _uow.HallRepository.RemoveRangeAsync(halls);
            }

            List<Film> films = cinema.Films.ToList();
            if (films.Any())
            {
                await _uow.FilmRepository.RemoveRangeAsync(films);
            }

            //TODO: delete reports in FileStorage
            List<Report> reports = cinema.WorkDays
                .Select(workday => workday.Report)
                .Where(report => report is not null)
                .ToList();
            if (reports.Any())
            {
                await _uow.ReportRepository.RemoveRangeAsync(reports);
            }

            List<Workday> workdays = cinema.WorkDays.ToList();
            if (workdays.Any())
            {
                await _uow.WorkdayRepository.RemoveRangeAsync(workdays);
            }

            await _uow.CinemaRepository.RemoveAsync(cinema);
        }
    }
}
