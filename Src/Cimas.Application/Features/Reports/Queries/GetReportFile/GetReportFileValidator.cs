using FluentValidation;

namespace Cimas.Application.Features.Reports.Queries.GetReportFile
{
    public class GetReportFileValidator : AbstractValidator<GetReportFileQuery>
    {
        public GetReportFileValidator()
        {
            RuleFor(x => x.ReportId)
                .NotEmpty();
        }
    }
}
