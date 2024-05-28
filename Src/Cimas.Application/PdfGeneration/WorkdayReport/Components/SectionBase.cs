using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Cimas.Application.PdfGeneration.WorkdayReport.Components
{
    public abstract class SectionBase : IComponent
    {
        protected static IContainer CellStyle(IContainer container) =>
            container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);

        public abstract void Compose(IContainer container);
    }
}
