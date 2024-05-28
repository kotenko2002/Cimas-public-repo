namespace Cimas.Domain.Models.Sessions
{
    public class SessionSeat
    {
        public Guid? TicketId { get; set; }
        public Guid SeatId { get; set; }

        public int Row { get; set; }
        public int Column { get; set; }
        public SessionSeatStatus Status { get; set; }
    }
}
