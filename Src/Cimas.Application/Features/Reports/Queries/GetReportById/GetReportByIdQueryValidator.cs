using FluentValidation;

namespace Cimas.Application.Features.Reports.Queries.GetReportById
{
    public class GetReportByIdQueryValidator : AbstractValidator<GetReportByIdQuery>
    {
        public GetReportByIdQueryValidator()
        {
            RuleFor(x => x.ReportId)
                .NotEmpty();
        }
    }
}
