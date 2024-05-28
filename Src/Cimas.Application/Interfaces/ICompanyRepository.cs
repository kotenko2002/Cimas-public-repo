using Cimas.Domain.Entities.Companies;

namespace Cimas.Application.Interfaces
{
    public interface ICompanyRepository : IBaseRepository<Company>
    {
        Task<Company> GetCompaniesIncludedUsersByIdAsync(Guid companyId);
    }
}
