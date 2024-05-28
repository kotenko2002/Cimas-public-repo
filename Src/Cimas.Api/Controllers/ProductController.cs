using Cimas.Api.Common.Extensions;
using Cimas.Api.Contracts.Products;
using Cimas.Application.Features.Products.Commands.CreateProduct;
using Cimas.Application.Features.Products.Commands.DeleteProduct;
using Cimas.Application.Features.Products.Commands.UpateProduct;
using Cimas.Application.Features.Products.Queries.GetProductsByCinemaId;
using Cimas.Domain.Entities.Products;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cimas.Api.Controllers
{
    [Route("products"), Authorize(Roles = Roles.Worker)]
    public class ProductController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductController(
          IMediator mediator,
          IHttpContextAccessor httpContextAccessor
        ) : base(mediator)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("{cinemaId}")]
        public async Task<IActionResult> CreateProduct(Guid cinemaId, CreateProductRequest request)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var command = (userIdResult.Value, cinemaId, request).Adapt<CreateProductCommand>();
            ErrorOr<Success> createProductResult = await _mediator.Send(command);

            return createProductResult.Match(
                NoContent,
                Problem
            );
        }

        [HttpGet("{cinemaId}")]
        public async Task<IActionResult> GetProductsByCinemaId(Guid cinemaId)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var command = new GetProductsByCinemaIdQuery(userIdResult.Value, cinemaId);
            ErrorOr<List<Product>> getProductsByCinemaIdResult = await _mediator.Send(command);

            return getProductsByCinemaIdResult.Match(
                products => Ok(products.Adapt<List<ProductResponse>>()),
                Problem
            );
        }

        [HttpPatch]
        public async Task<IActionResult> UpateProducts(UpdateProductsRequest request)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var command = (userIdResult.Value, request).Adapt<UpateProductsCommand>();
            ErrorOr<Success> upateProductsResult = await _mediator.Send(command);

            return upateProductsResult.Match(
                NoContent,
                Problem
            );
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var command = new DeleteProductCommand(userIdResult.Value, productId);
            ErrorOr<Success> deleteProductIdResult = await _mediator.Send(command);

            return deleteProductIdResult.Match(
                NoContent,
                Problem
            );
        }
    }
}
