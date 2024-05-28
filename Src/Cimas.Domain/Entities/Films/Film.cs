using Cimas.Domain.Entities.Cinemas;
using Cimas.Domain.Entities.Sessions;

namespace Cimas.Domain.Entities.Films
{
    public class Film : BaseEntity
    {
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }

        public bool IsDeleted { get; set; }

        public Guid CinemaId { get; set; }
        public virtual Cinema Cinema { get; set; }

        public virtual ICollection<Session> Sessions { get; set; }
    }
}
