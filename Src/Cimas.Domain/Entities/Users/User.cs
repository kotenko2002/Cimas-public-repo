using Cimas.Domain.Entities.Companies;
using Cimas.Domain.Entities.WorkDays;
using Microsoft.AspNetCore.Identity;

namespace Cimas.Domain.Entities.Users
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public bool IsFired { get; set; }

        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<Workday> WorkDays { get; set; }
    }
}
