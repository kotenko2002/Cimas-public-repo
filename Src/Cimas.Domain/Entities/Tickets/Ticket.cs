using Cimas.Domain.Entities.Halls;
using Cimas.Domain.Entities.Sessions;

namespace Cimas.Domain.Entities.Tickets
{
    public class Ticket : BaseEntity
    {
        public DateTime CreationTime { get; set; }
        public TicketStatus Status { get; set; }

        public Guid SessionId { get; set; }
        public virtual Session Session { get; set; }
        public Guid SeatId { get; set; }
        public virtual HallSeat Seat { get; set; }
    }
}
