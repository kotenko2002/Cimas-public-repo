using Cimas.Domain.Entities.Users;

namespace Cimas.Domain.Models.Auth
{
    public class AuthModel
    {
        public string AccessToken { get; set; }
        public Token RefreshToken { get; set; }

        public User User { get; set; }
        public string[] Roles { get; set; }
    }
}
