namespace Cimas.Application.PdfGeneration.Tickets
{
    public record TicketPdfItem(
        string CinemaName,
        string CinemaAdress,
        string FilmName,
        string SessionDate,
        string SessionTime,
        string HallName,
        string Price,
        string Row,
        string Column
    );
}
