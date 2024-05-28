namespace Cimas.Application.Interfaces
{
    public interface IUnitOfWork
    {
        ICompanyRepository CompanyRepository { get; }
        IUserRepository UserRepository { get; }
        ICinemaRepository CinemaRepository { get; }
        IHallRepository HallRepository { get; }
        ISeatRepository SeatRepository { get; }
        IFilmRepository FilmRepository { get; }
        ISessionRepository SessionRepository { get; }
        ITicketRepository TicketRepository { get; }
        IWorkdayRepository WorkdayRepository { get; }
        IReportRepository ReportRepository { get; }
        IProductRepository ProductRepository { get; }
        
        Task CompleteAsync();
    }
}
