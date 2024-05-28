namespace Cimas.Api.Contracts.Halls
{
    public record CreateHallRequest(
        string Name,
        int NumberOfRows,
        int NumberOfColumns
    );
}
