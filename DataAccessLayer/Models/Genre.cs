using DataAccessLayer.Models.Joins;

namespace DataAccessLayer.Models
{
    public class Genre
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<MovieGenre> MoviesGenres { get; set; }
    }
}
