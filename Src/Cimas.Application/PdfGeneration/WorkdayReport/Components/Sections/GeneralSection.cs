using Cimas.Domain.Entities.WorkDays;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Cimas.Application.PdfGeneration.WorkdayReport.Components.Sections
{
    internal class GeneralSection : SectionBase
    {
        private readonly Workday _workday;
        private readonly decimal _profit;

        public GeneralSection(Workday workday, decimal profit)
        {
            _workday = workday;
            _profit = profit;
        }

        public override void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column
                    .Item()
                    .Component(new SectionHeader("Загальне"));

                column.Item().Background(Colors.Grey.Lighten3).Padding(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                    });

                    Dictionary<string, string> items = new Dictionary<string, string>()
                    {
                        {"Назва кінотеатру", _workday.Cinema.Name},
                        {"Адреса кінотеатру", _workday.Cinema.Address},
                        {"Працівник", $"{_workday.User.LastName} {_workday.User.FirstName}"},
                        {"Початок робочого дня", _workday.StartDateTime.ToString("dd/MM/yyyy HH:mm")},
                        {"Кінець робочго дня", FormatNullableDateTime(_workday.EndDateTime, "dd/MM/yyyy HH:mm")},
                        {"Прибуток", $"{_profit}грн"}
                    };

                    foreach (var item in items)
                    {
                        AddInfoSectionAsCell(table, item.Key, item.Value);
                    }
                });
            });
        }

        private void AddInfoSectionAsCell(TableDescriptor table, string title, string value)
        {
            table.Cell().Element(CellStyle).Text(text =>
            {
                text.Span(title).Bold();
                text.Span(": ").Bold();
                text.Span(value);
            });
        }

        private static string FormatNullableDateTime(DateTime? dateTime, string format)
            => dateTime?.ToString(format) ?? "N/A";
    }
}
