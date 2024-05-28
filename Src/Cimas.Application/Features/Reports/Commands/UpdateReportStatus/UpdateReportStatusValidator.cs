using Cimas.Application.Common.Extensions;
using FluentValidation;

namespace Cimas.Application.Features.Reports.Commands.UpdateReportStatus
{
    public class UpdateReportStatusValidator : AbstractValidator<UpdateReportStatusCommand>
    {
        public UpdateReportStatusValidator()
        {
            RuleFor(x => x.ReportId)
               .NotEmpty();

            RuleFor(x => x.Status)
                .MustBeValidEnum(status => status);
        }
    }
}
