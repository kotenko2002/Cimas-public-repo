using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Cimas.Application.PdfGeneration.Tickets
{
    public class TicketPdf : IDocument
    {
        private readonly List<TicketPdfItem> _tickets;

        public TicketPdf(List<TicketPdfItem> tickets)
        {
            _tickets = tickets;
        }

        public void Compose(IDocumentContainer container)
        {
            foreach (var ticket in _tickets)
            {
                container
                    .Page(page =>
                    {
                        var pageSize = new PageSize(13.97f, 5.08f, Unit.Centimetre);
                        page.Size(pageSize.Landscape());
                        page.Margin(5);
                        page.DefaultTextStyle(x => x.FontFamily("DejaVu Sans"));

                        page.Content().Element(content => ComposeContent(content, ticket));
                    });
            }
        }

        private void ComposeContent(IContainer container, TicketPdfItem ticket)
        {
            container.Column(column =>
            {
                column.Item()
                    .AlignCenter()
                    .Text($"\"{ticket.CinemaName}\" за адресою \"{ticket.CinemaAdress}\"")
                    .FontSize(8);

                column.Item()
                    .PaddingTop(5)
                    .AlignCenter()
                    .Text(ticket.FilmName)
                    .FontSize(12)
                    .Bold();

                column.Item().PaddingTop(5).AlignCenter().Column(innerColumn =>
                {
                    innerColumn.Item()
                        .Text(text =>
                        {
                            text.Span("дата: ").FontSize(8);
                            text.Span(ticket.SessionDate).FontSize(8).Bold();
                            
                        });
                    innerColumn.Item()
                      .Text(text =>
                      {
                          text.Span("початок: ").FontSize(8);
                          text.Span(ticket.SessionTime).FontSize(8).Bold();

                      });
                    innerColumn.Item()
                      .Text(text =>
                      {
                          text.Span("зал: ").FontSize(8);
                          text.Span(ticket.HallName).FontSize(8).Bold();

                      });
                });

                column.Item().PaddingTop(10).Row(innerRow =>
                {
                    innerRow.RelativeItem()
                        .PaddingLeft(10)
                        .AlignLeft()
                        .AlignMiddle()
                        .Text(text =>
                        {
                            text.Span("Ціна: ").FontSize(10);
                            text.Span($"{ticket.Price}грн").FontSize(10).Bold();

                        });

                    innerRow.RelativeItem()
                        .AlignCenter()
                        .Width(50)
                        .Image(ApplicationResource.filmreel);

                    innerRow.RelativeItem()
                        .PaddingRight(10)
                        .AlignRight()
                        .AlignMiddle()
                         .Text(text =>
                         {
                             text.Span("Ряд: ").FontSize(10);
                             text.Span(ticket.Row).FontSize(10).Bold();
                             text.Span(" ");
                             text.Span("Місце: ").FontSize(10);
                             text.Span(ticket.Column).FontSize(10).Bold();
                         });
                });
            });
        }
    }
}
