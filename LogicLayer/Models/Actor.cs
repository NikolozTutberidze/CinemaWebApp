using Cinema.Domain.Models.Joins;

namespace Cinema.Domain.Models
{
    public class Actor
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Origin { get; set; }
        public List<MovieActor> MoviesActors { get; set; }
    }
}
