using Cimas.Domain.Models;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Tickets.Commands.GenerateTicketsFile
{
    public record GenerateTicketsFileCommand(
        List<Guid> IdsOfSoldTickets
    ) : IRequest<ErrorOr<FileDownloadResult>>;
}
