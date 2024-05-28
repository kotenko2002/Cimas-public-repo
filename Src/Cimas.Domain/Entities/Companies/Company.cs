using Cimas.Domain.Entities.Cinemas;
using Cimas.Domain.Entities.Users;

namespace Cimas.Domain.Entities.Companies
{
    public class Company : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<Cinema> Cinemas { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
