using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.WorkDays;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Workdays.Queries.GetCurrentWorkday
{
    public class GetCurrentWorkdayHandler : IRequestHandler<GetCurrentWorkdayQuery, ErrorOr<Workday>>
    {
        private readonly IUnitOfWork _uow;

        public GetCurrentWorkdayHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<Workday>> Handle(GetCurrentWorkdayQuery query, CancellationToken cancellationToken)
        {
            return await _uow.WorkdayRepository.GetWorkdayByUserIdAsync(query.UserId);
        }
    }
}
