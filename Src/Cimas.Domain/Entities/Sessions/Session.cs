using Cimas.Domain.Entities.Films;
using Cimas.Domain.Entities.Halls;
using Cimas.Domain.Entities.Tickets;

namespace Cimas.Domain.Entities.Sessions
{
    public class Session : BaseEntity
    {
        public DateTime StartDateTime { get; set; }
        public decimal Price { get; set; }

        public Guid HallId { get; set; }
        public virtual Hall Hall { get; set; }
        public Guid FilmId { get; set; }
        public virtual Film Film { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
