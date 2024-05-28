using Cimas.Application.Interfaces;
using Cimas.Application.PdfGeneration.Tickets;
using Cimas.Domain.Entities.Tickets;
using Cimas.Domain.Models;
using ErrorOr;
using MediatR;
using QuestPDF.Fluent;
using System.Globalization;

namespace Cimas.Application.Features.Tickets.Commands.GenerateTicketsFile
{
    public class GenerateTicketsFileHandler : IRequestHandler<GenerateTicketsFileCommand, ErrorOr<FileDownloadResult>>
    {
        private readonly IUnitOfWork _uow;

        public GenerateTicketsFileHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<FileDownloadResult>> Handle(GenerateTicketsFileCommand command, CancellationToken cancellationToken)
        {
            List<Ticket> soldTickets = await _uow.TicketRepository
                .GetTicketsWithFullIncludesForPdf(command.IdsOfSoldTickets);

            List<TicketPdfItem> ticketsAsTicketPdfItems = soldTickets
                .OrderBy(t => t.Seat.Row)
                .ThenBy(t => t.Seat.Column)
                .Select(t => new TicketPdfItem(
                    t.Session.Hall.Cinema.Name,
                    t.Session.Hall.Cinema.Address,
                    t.Session.Film.Name,
                    t.Session.StartDateTime.ToString("D", new CultureInfo("uk-UA")),
                    t.Session.StartDateTime.ToString("HH:mm"),
                    t.Session.Hall.Name,
                    t.Session.Price.ToString(),
                    (t.Seat.Row + 1).ToString(),
                    (t.Seat.Column + 1).ToString()
                )).ToList();


            var ticketPdf = new TicketPdf(ticketsAsTicketPdfItems);

            var memoryStream = new MemoryStream();
            ticketPdf.GeneratePdf(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new FileDownloadResult()
            {
                Stream = memoryStream,
                FileName = "tickets",
                ContentType = "application/pdf"
            };
        }
    }
}
