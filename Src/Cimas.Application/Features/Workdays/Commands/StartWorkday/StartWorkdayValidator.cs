using FluentValidation;

namespace Cimas.Application.Features.Workdays.Commands.StartWorkday
{
    public class StartWorkdayValidator : AbstractValidator<StartWorkdayCommand>
    {
        public StartWorkdayValidator()
        {
            RuleFor(x => x.CinemaId)
                .NotEmpty();
        }
    }
}
