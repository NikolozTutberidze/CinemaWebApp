using CinemaApiSql.Models.Joins;

namespace CinemaApiSql.Dtos
{
    public class GenreDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<MovieGenre> MoviesGenres { get; set; }
    }
}
