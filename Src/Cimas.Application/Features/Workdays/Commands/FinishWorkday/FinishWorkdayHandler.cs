using Cimas.Application.Interfaces;
using Cimas.Application.PdfGeneration.WorkdayReport;
using Cimas.Domain.Entities.Cinemas;
using Cimas.Domain.Entities.Reports;
using Cimas.Domain.Entities.Tickets;
using Cimas.Domain.Entities.WorkDays;
using ErrorOr;
using MediatR;
using QuestPDF.Fluent;

namespace Cimas.Application.Features.Workdays.Commands.FinishWorkday
{
    public class FinishWorkdayHandler : IRequestHandler<FinishWorkdayCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileStorageService _fileStorageService;

        public FinishWorkdayHandler(
            IUnitOfWork uow,
            IFileStorageService fileStorageService)
        {
            _uow = uow;
            _fileStorageService = fileStorageService;
        }

        public async Task<ErrorOr<Success>> Handle(FinishWorkdayCommand command, CancellationToken cancellationToken)
        {
            DateTime endDateTime = DateTime.UtcNow;
            
            Workday unfinishedWorkday =  await _uow.WorkdayRepository.GetWorkdayByUserIdAsync(command.UserId);
            if(unfinishedWorkday is null)
            {
                return Error.Failure(description: "User does not have an unfinished workday");
            }

            Workday workdayFullDataForReport =  await GetWorkdayWithFullDataForReport(unfinishedWorkday.Id, endDateTime);
            decimal profit = CalculateProfit(workdayFullDataForReport.Cinema);

            ErrorOr<string> reportFileStorageId = await GenerateAndUploadReport(workdayFullDataForReport, profit);
            if (reportFileStorageId.IsError)
            {
                return reportFileStorageId.Errors;
            }

            unfinishedWorkday.EndDateTime = endDateTime;
            await _uow.ReportRepository.AddAsync(new Report()
            {
                WorkDay = unfinishedWorkday,
                FileId = reportFileStorageId.Value,
                Status = ReportStatus.NotReviewed
            });
            await UpdateProductQuantities(unfinishedWorkday.CinemaId);

            await _uow.CompleteAsync();

            return Result.Success;
        }

        private async Task<Workday> GetWorkdayWithFullDataForReport(Guid workdayId, DateTime? endDateTime)
        {
            Workday workdayWithFullData = await _uow.WorkdayRepository.GetWorkdayWithBaseDataForReportByIdAsync(workdayId);
            workdayWithFullData.EndDateTime = endDateTime;

            List<Ticket> tickets = await _uow.TicketRepository.GetTicketsByDateTimeRangeAndCinemaIdAsync(
                workdayWithFullData.StartDateTime,
                workdayWithFullData.EndDateTime.Value,
                workdayWithFullData.CinemaId);

            workdayWithFullData.Cinema.Films = tickets
                .Select(ticket => ticket.Session.Film)
                .DistinctBy(film => film.Id)
                .ToList();

            return workdayWithFullData;
        }

        private decimal CalculateProfit(Cinema cinema)
        {
            decimal profitFromSoldProducts = cinema.Products
                .Select(product => product.Price * product.SoldAmount)
                .Sum();

            decimal profitFromSoldTickets = cinema.Films
                .SelectMany(film => film.Sessions)
                .Select(session => session.Price * session.Tickets.Count(ticket => ticket.Status == TicketStatus.Sold))
                .Sum();

            return profitFromSoldProducts + profitFromSoldTickets;
        }

        private async Task<ErrorOr<string>> GenerateAndUploadReport(Workday workdayWithFullData, decimal profit)
        {
            var reportPdf = new WorkdayReportPdf(workdayWithFullData, profit);

            using var stream = new MemoryStream();

            reportPdf.GeneratePdf(stream);
            stream.Seek(0, SeekOrigin.Begin);

            return await _fileStorageService.UploadFileAsync(stream, "Report", "application/pdf");
        }

        private async Task UpdateProductQuantities(Guid cinemaId)
        {
            var products = await _uow.ProductRepository.GetProductsByCinemaId(cinemaId);
            foreach (var product in products)
            {
                product.Amount += product.IncomeAmount - product.SoldAmount;
                product.SoldAmount = 0;
                product.IncomeAmount = 0;
            }
        }
    }
}
