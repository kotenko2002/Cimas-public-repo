using Cimas.Application.Common.Extensions;
using FluentValidation;

namespace Cimas.Application.Features.Tickets.Commands.UpdateTickets
{
    public class UpdateTicketsValidator : AbstractValidator<UpdateTicketsCommand>
    {
        public UpdateTicketsValidator()
        {
            RuleFor(x => x.Tickets)
                .NotEmpty()
                .MustHaveUniqueIds(ticket => ticket.TicketId)
                .MustBeValidEnum(ticket => ticket.Status);
        }
    }
}
