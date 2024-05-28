using FluentValidation;

namespace Cimas.Application.Features.Tickets.Commands.DeleteTickets
{
    public class DeleteTicketsValidator : AbstractValidator<DeleteTicketsCommand>
    {
        public DeleteTicketsValidator()
        {
            RuleFor(x => x.TicketIds)
                .NotEmpty()
                .Must(tickets => tickets.Count > 0)
                .WithMessage("'TicketIds' must contain at least 1 Id");
        }
    }
}
