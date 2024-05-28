using Cimas.Application.PdfGeneration.WorkdayReport.Components.Sections;
using Cimas.Domain.Entities.Products;
using Cimas.Domain.Entities.Sessions;
using Cimas.Domain.Entities.WorkDays;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Cimas.Application.PdfGeneration.WorkdayReport
{
    public class WorkdayReportPdf : IDocument
    {
        private readonly Workday _workday;
        private readonly decimal _profit;

        public WorkdayReportPdf(Workday workday, decimal profit)
        {
            _workday = workday;
            _profit = profit;
        }

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(25);
                    page.DefaultTextStyle(x => x.FontFamily("DejaVu Sans"));

                    page.Content().Element(ComposeContent);
                    page.Footer().Element(ComposeFooter);
                });
        }

        private void ComposeContent(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(25);

                List<Product> products = _workday.Cinema.Products.ToList();
                List<Session> sessions = _workday.Cinema.Films
                   .SelectMany(film => film.Sessions)
                   .ToList();

                column
                    .Item()
                    .Component(new GeneralSection(_workday, _profit));

                column
                    .Item()
                    .Component(new ProductsSection(products));

                column
                    .Item()
                    .Component(new SessionsSection(sessions));
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Text(x =>
                {
                    x.CurrentPageNumber();
                    x.Span(" з ");
                    x.TotalPages();
                });
            });
        }
    }
}
