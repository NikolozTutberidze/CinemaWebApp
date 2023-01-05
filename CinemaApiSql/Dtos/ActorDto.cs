using CinemaApiSql.Models.Joins;

namespace CinemaApiSql.Dtos
{
    public class ActorDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Origin { get; set; }
        public List<MovieActor> MoviesActors { get; set; }
    }
}
