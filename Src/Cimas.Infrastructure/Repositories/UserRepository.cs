using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Users;
using Cimas.Infrastructure.Common;

namespace Cimas.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(CimasDbContext context) : base(context)
        {
        }
    }
}
