using Cinema.Domain.Models.Joins;

namespace Cinema.Domain.Models
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public double IMDBRate { get; set; }
        public string Review { get; set; }
        public Guid DirectorId { get; set; }
        public Director Director { get; set; }
        public List<MovieActor> MoviesActors { get; set; }
        public List<MovieGenre> MoviesGenres { get; set; }
    }
}
