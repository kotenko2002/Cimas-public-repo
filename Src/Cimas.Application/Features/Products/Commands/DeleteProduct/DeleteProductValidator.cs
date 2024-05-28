using FluentValidation;

namespace Cimas.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductValidator()
        {
            RuleFor(x => x.ProductId)
               .NotEmpty();
        }
    }
}
