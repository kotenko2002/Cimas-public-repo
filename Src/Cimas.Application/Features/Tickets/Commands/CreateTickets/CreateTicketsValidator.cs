using Cimas.Application.Common.Extensions;
using FluentValidation;

namespace Cimas.Application.Features.Tickets.Commands.CreateTickets
{
    public class CreateTicketsValidator : AbstractValidator<CreateTicketsCommand>
    {
        public CreateTicketsValidator()
        {
            RuleFor(x => x.Tickets)
                .NotEmpty()
                .MustHaveUniqueIds(ticket => ticket.SeatId)
                .MustBeValidEnum(ticket => ticket.Status);
        }
    }
}
