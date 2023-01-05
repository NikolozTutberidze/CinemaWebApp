namespace CinemaApiSql.Models.Joins
{
    public class MovieGenre
    {
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }
        public Guid GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
