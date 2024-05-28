using FluentValidation;

namespace Cimas.Application.Features.Tickets.Commands.GenerateTicketsFile
{
    public class GenerateTicketsFileValidator : AbstractValidator<GenerateTicketsFileCommand>
    {
        public GenerateTicketsFileValidator()
        {
            RuleFor(x => x.IdsOfSoldTickets)
                .NotEmpty();
        }
    }
}
