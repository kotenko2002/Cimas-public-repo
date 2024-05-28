using Cimas.Application.Common.Extensions;
using FluentValidation;

namespace Cimas.Application.Features.Products.Commands.UpateProduct
{
    public class UpateProductValidator : AbstractValidator<UpateProductsCommand>
    {
        public UpateProductValidator()
        {
            RuleFor(x => x.Products)
                .NotEmpty()
                .MustHaveUniqueIds(product => product.Id)
                .DependentRules(() =>
                {
                    RuleForEach(x => x.Products)
                        .ChildRules(product =>
                        {
                            product
                                .RuleFor(x => x.Id)
                                .NotEmpty();

                            product
                                .RuleFor(x => x.Name)
                                .NotEmpty();

                            product
                                .RuleFor(x => x.Price)
                                .GreaterThanOrEqualTo(0);

                            product
                                .RuleFor(x => x.Amount)
                                .GreaterThanOrEqualTo(0);

                            product
                                .RuleFor(x => x.SoldAmount)
                                .GreaterThanOrEqualTo(0);

                            product
                                .RuleFor(x => x.IncomeAmount)
                                .GreaterThanOrEqualTo(0);

                            // add custom rule: Amount + IncomeAmount cant be < SoldAmount
                        });
                });
        }
    }
}
