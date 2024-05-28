using Cimas.Domain.Entities.Users;

namespace Cimas.Domain.Models
{
    public class UserWithRoles : User
    {
        public string[] Roles { get; set; }
    }
}
