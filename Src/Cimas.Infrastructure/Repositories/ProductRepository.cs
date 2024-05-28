using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Products;
using Cimas.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Cimas.Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(CimasDbContext context) : base(context) {}

        public async Task<Product> GetProductIncludedCinemaByIdAsync(Guid productId)
        {
            return await Sourse
                .Include(product => product.Cinema)
                .FirstOrDefaultAsync(product => product.Id == productId);
        }

        public async Task<List<Product>> GetProductsByIdsAsync(List<Guid> ids)
        {
            return await Sourse
               .Where(seat => ids.Contains(seat.Id))
               .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByCinemaId(Guid cinemaId)
        {
            return await Sourse
                .Where(produt => produt.CinemaId == cinemaId)
                .ToListAsync();
        }
    }
}
