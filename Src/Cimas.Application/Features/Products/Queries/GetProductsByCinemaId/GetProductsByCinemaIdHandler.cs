using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Cinemas;
using Cimas.Domain.Entities.Products;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Products.Queries.GetProductsByCinemaId
{
    public class GetProductsByCinemaIdHandler : IRequestHandler<GetProductsByCinemaIdQuery, ErrorOr<List<Product>>>
    {
        private readonly IUnitOfWork _uow;

        public GetProductsByCinemaIdHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<List<Product>>> Handle(GetProductsByCinemaIdQuery query, CancellationToken cancellationToken)
        {
            Cinema cinema = await _uow.CinemaRepository.GetCinemaIncludedProductsByIdAsync(query.CinemaId);
            if (cinema is null)
            {
                return Error.NotFound(description: "Cinema with such id does not exist");
            }

            User user = await _uow.UserRepository.GetByIdAsync(query.UserId);
            if (user.CompanyId != cinema.CompanyId)
            {
                return Error.Forbidden(description: "You do not have the necessary permissions to perform this action");
            }

            return cinema.Products.ToList();
        }
    }
}
