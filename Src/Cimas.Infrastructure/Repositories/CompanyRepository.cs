using Cimas.Infrastructure.Common;
using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Companies;
using Microsoft.EntityFrameworkCore;

namespace Cimas.Infrastructure.Repositories
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(CimasDbContext context) : base(context) {}

        public async Task<Company> GetCompaniesIncludedUsersByIdAsync(Guid companyId)
        {
            return await Sourse
                .Where(company => company.Id == companyId)
                .Select(company => new Company()
                {
                    Id = company.Id,
                    Name = company.Name,
                    Cinemas = company.Cinemas,
                    Users = company.Users.Where(user => !user.IsFired).ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}
