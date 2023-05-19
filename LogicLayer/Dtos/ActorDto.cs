using DataAccessLayer.Models.Joins;

namespace LogicLayer.Dtos
{
    public class ActorDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Origin { get; set; }
        public List<MoviesForActorDto> Movies { get; set; }
    }
}
