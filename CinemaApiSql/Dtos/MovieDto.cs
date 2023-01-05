using CinemaApiSql.Models;
using CinemaApiSql.Models.Joins;

namespace CinemaApiSql.Dtos
{
    public class MovieDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public double IMDBRate { get; set; }
        public string Review { get; set; }
        public Guid DirectorId { get; set; }
        public List<MovieActor> MoviesActors { get; set; }
        public List<MovieGenre> MoviesGenres { get; set; }
    }
}
