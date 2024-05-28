namespace Cimas.Api.Contracts.Halls
{
    public record HallResponse(
       Guid Id,
       string Name,
       int NumberOfSeats
    );
}
