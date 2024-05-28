using Cimas.Domain.Entities.Cinemas;

namespace Cimas.Domain.Entities.Products
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public int Amount { get; set; }
        public int SoldAmount { get; set; }
        public int IncomeAmount { get; set; }

        public Guid CinemaId { get; set; }
        public virtual Cinema Cinema { get; set; }
    }
}
