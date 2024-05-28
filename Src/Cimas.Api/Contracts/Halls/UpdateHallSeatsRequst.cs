using Cimas.Domain.Entities.Halls;

namespace Cimas.Api.Contracts.Halls
{
    public record UpdateHallSeatsRequst(List<HallSeatModel> Seats);

    public record HallSeatModel(
        Guid Id,
        HallSeatStatus Status
    );
}
