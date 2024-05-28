using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Products;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _uow;

        public DeleteProductHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<Success>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            Product product = await _uow.ProductRepository.GetProductIncludedCinemaByIdAsync(command.ProductId);
            if (product is null)
            {
                return Error.NotFound(description: "Product with such id does not exist");
            }

            User user = await _uow.UserRepository.GetByIdAsync(command.UserId);
            if (user.CompanyId != product.Cinema.CompanyId)
            {
                return Error.Forbidden(description: "You do not have the necessary permissions to perform this action");
            }

            await _uow.ProductRepository.RemoveAsync(product);

            await _uow.CompleteAsync();

            return Result.Success;
        }
    }
}
