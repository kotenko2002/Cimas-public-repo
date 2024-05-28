using Cimas.Application.Common.Extensions;
using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Cinemas;
using Cimas.Domain.Entities.Products;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Products.Commands.UpateProduct
{
    public class UpateProductHandler : IRequestHandler<UpateProductsCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _uow;

        public UpateProductHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<Success>> Handle(UpateProductsCommand command, CancellationToken cancellationToken)
        {
            if(command.Products.Any(product => product.Amount + product.IncomeAmount < product.SoldAmount))
            {
                return Error.Validation(description: $"Сума полів \"Кількість\" та \"Надійшло\" не може бути меншою за значення поля \"Продано\"");
            }

            List<Guid> productIds = command.Products.Select(product => product.Id).ToList();
            List<Product> products = await _uow.ProductRepository.GetProductsByIdsAsync(productIds);

            var invalidProductt = command.Products
                .FirstOrDefault(commandSeat => !products.Any(s => s.Id == commandSeat.Id));
            if (invalidProductt != null)
            {
                return Error.NotFound(description: $"Product with id '{invalidProductt.Id}' does not exist");
            }

            Guid? cinemaId = products.GetSingleDistinctIdOrNull(ticket => ticket.CinemaId);
            if (!cinemaId.HasValue)
            {
                return Error.Failure(description: "Cinema Ids are not the same in all products");
            }

            Cinema cinema = await _uow.CinemaRepository.GetByIdAsync(cinemaId.Value);
            User user = await _uow.UserRepository.GetByIdAsync(command.UserId);
            if (user.CompanyId != cinema.CompanyId)
            {
                return Error.Forbidden(description: "You do not have the necessary permissions to perform this action");
            }

            Dictionary<Guid, UpdateProductModel> updatedProducts = command.Products
                .ToDictionary(product => product.Id, product => product);
            foreach (Product product in products)
            {
                UpdateProductModel updatedProduct = updatedProducts[product.Id];

                product.Name = updatedProduct.Name;
                product.Price = updatedProduct.Price;
                product.Amount = updatedProduct.Amount;
                product.SoldAmount = updatedProduct.SoldAmount;
                product.IncomeAmount = updatedProduct.IncomeAmount;
            }

            await _uow.CompleteAsync();

            return Result.Success;
        }
    }
}
