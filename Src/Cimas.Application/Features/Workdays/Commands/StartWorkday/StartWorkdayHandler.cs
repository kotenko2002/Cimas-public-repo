using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Cinemas;
using Cimas.Domain.Entities.Users;
using Cimas.Domain.Entities.WorkDays;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Workdays.Commands.StartWorkday
{
    public class StartWorkdayHandler : IRequestHandler<StartWorkdayCommand, ErrorOr<Workday>>
    {
        private readonly IUnitOfWork _uow;

        public StartWorkdayHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<Workday>> Handle(StartWorkdayCommand command, CancellationToken cancellationToken)
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

            Workday unfinishedWorkday = await _uow.WorkdayRepository.GetWorkdayByUserIdAsync(command.UserId);
            if (unfinishedWorkday is not null)
            {
                return Error.Failure(description: "User has an unfinished workday");
            }

            Workday unfinishedWorkdayOfAnotherUser = await _uow.WorkdayRepository.GetNotFinishedWorkdayByCinemaIdAsync(cinema.Id);
            if (unfinishedWorkdayOfAnotherUser is not null)
            {
                return Error.Failure(description: "Інший працівник вже розпочав робочий день у цьому кінотеатрі");
            }

            var workday = new Workday()
            {
                StartDateTime = DateTime.UtcNow,
                Cinema = cinema,
                User = user,
            };

            await _uow.WorkdayRepository.AddAsync(workday);

            await _uow.CompleteAsync();

            return workday;
        }
    }
}
