using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Companies;
using Cimas.Domain.Entities.Users;
using Cimas.Domain.Models;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Cimas.Application.Features.Users.Queries.GetCompanyUsers
{
    public class GetCompanyUsersHandler : IRequestHandler<GetCompanyUsersQuery, ErrorOr<List<UserWithRoles>>>
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<User> _userManager;

        public GetCompanyUsersHandler(
            IUnitOfWork uow,
            UserManager<User> userManager)
        {
            _uow = uow;
            _userManager = userManager;
        }

        public async Task<ErrorOr<List<UserWithRoles>>> Handle(GetCompanyUsersQuery query, CancellationToken cancellationToken)
        {
            User owner = await _uow.UserRepository.GetByIdAsync(query.OwnerId);
            
            Company company = await _uow.CompanyRepository.GetCompaniesIncludedUsersByIdAsync(owner.CompanyId);

            List<User> users = company.Users
                .Where(user => user.Id != owner.Id)
                .ToList();

            List<UserWithRoles> userWithRoles = [];
            foreach (var user in users)
            {
                IList<string> userRoles = await _userManager.GetRolesAsync(user);

                userWithRoles.Add(new UserWithRoles()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = userRoles.ToArray()
                });
            }

            return userWithRoles;
        }
    }
}
