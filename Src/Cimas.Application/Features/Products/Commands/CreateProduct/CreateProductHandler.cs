using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Cinemas;
using Cimas.Domain.Entities.Products;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _uow;

        public CreateProductHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<Success>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            Cinema cinema = await _uow.CinemaRepository.GetByIdAsync(command.CinemaId);
            if (cinema is null)
            {
                return Error.NotFound(description: "Cinema with such id does not exist");
            }

            User user = await _uow.UserRepository.GetByIdAsync(command.UserId);
            if (user.CompanyId != cinema.CompanyId)
            {
                return Error.Forbidden(description: "You do not have the necessary permissions to perform this action");
            }

            var product = new Product()
            {
                Name = command.Name,
                Price = command.Price,
                Amount = 0,
                SoldAmount = 0,
                IncomeAmount = 0,
                Cinema = cinema
            };
            await _uow.ProductRepository.AddAsync(product);

            await _uow.CompleteAsync();

            return Result.Success;
        }
    }
}
