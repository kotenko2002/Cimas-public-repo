using Cimas.Domain.Entities.Products;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Cimas.Application.PdfGeneration.WorkdayReport.Components.Sections
{
    public class ProductsSection : SectionBase
    {
        private readonly List<Product> _products;

        public ProductsSection(List<Product> products)
        {
            _products = products;
        }

        public override void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column
                    .Item()
                    .Component(new SectionHeader("Товари"));

                column.Item().Background(Colors.Grey.Lighten3).Padding(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(25);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("#");
                        header.Cell().Text("Назва").Bold();
                        header.Cell().AlignCenter().Text("Кількість").Bold();
                        header.Cell().AlignCenter().Text("Надійшло").Bold();
                        header.Cell().AlignCenter().Text("Продано").Bold();
                        header.Cell().AlignCenter().Text("Ціна за шт.").Bold();
                    });

                    int index = 1;
                    foreach (var product in _products)
                    {
                        table.Cell().Element(CellStyle).AlignMiddle()
                            .Text($"{index}");

                        table.Cell().Element(CellStyle).AlignMiddle()
                            .Text(product.Name);

                        table.Cell().Element(CellStyle).AlignCenter().AlignMiddle()
                            .Text(product.Amount.ToString());

                        table.Cell().Element(CellStyle).AlignCenter().AlignMiddle()
                            .Text(product.IncomeAmount.ToString());

                        table.Cell().Element(CellStyle).AlignCenter().AlignMiddle()
                            .Text(product.SoldAmount.ToString());

                        table.Cell().Element(CellStyle).AlignCenter().AlignMiddle()
                            .Text($"{product.Price}грн");

                        index++;
                    }
                });
            });
        }
    }
}
