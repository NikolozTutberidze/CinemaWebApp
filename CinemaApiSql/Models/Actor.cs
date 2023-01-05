using CinemaApiSql.Models.Joins;

namespace CinemaApiSql.Models
{
    public class Actor
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Origin { get; set; }
        public List<MovieActor> MoviesActors { get; set; }
    }
}
