using Cimas.Domain.Entities.Halls;

namespace Cimas.Api.Contracts.Halls
{
    public record SeatResponse(
        Guid Id,
        int Row,
        int Column,
        HallSeatStatus Status
    );
}
