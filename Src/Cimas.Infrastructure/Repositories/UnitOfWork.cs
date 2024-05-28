using Cimas.Application.Interfaces;
using Cimas.Infrastructure.Common;

namespace Cimas.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly CimasDbContext _context;

        public ICompanyRepository CompanyRepository { get; }
        public IUserRepository UserRepository { get; }
        public ICinemaRepository CinemaRepository { get; }
        public IHallRepository HallRepository { get; }
        public ISeatRepository SeatRepository { get; }
        public IFilmRepository FilmRepository { get; }
        public ISessionRepository SessionRepository { get; }
        public ITicketRepository TicketRepository { get; }
        public IWorkdayRepository WorkdayRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IReportRepository ReportRepository { get; }
        

        public UnitOfWork(CimasDbContext context)
        {
            _context = context;

            CompanyRepository = new CompanyRepository(_context);
            CinemaRepository = new CinemaRepository(_context);
            HallRepository = new HallRepository(_context);
            SeatRepository = new SeatRepository(_context);
            FilmRepository = new FilmRepository(_context);
            SessionRepository = new SessionRepository(_context);
            TicketRepository = new TicketRepository(_context);
            UserRepository = new UserRepository(_context);
            WorkdayRepository = new WorkdayRepository(_context);
            ProductRepository = new ProductRepository(_context);
            ReportRepository = new ReportRepository(_context);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
