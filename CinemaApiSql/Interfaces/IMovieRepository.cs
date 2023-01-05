using CinemaApiSql.Dtos;
using CinemaApiSql.Models;

namespace CinemaApiSql.Interfaces
{
    public interface IMovieRepository
    {

        public void AddMovie(AddMovieDto request);
        public ICollection<MovieDto> GetMovies();
        public MovieDto GetMovieById(Guid requestId);
        public ICollection<MovieDto> GetMovieByTitle(string requestTitle);
        public ICollection<MovieDto> GetMovieByYear(int firstYear, int secondYear);
        public ICollection<MovieDto> GetMovieByIMDB(double firstRating, double secondRating);
        public void ChangeMovie(ChangeMovieDto request);
        public void DeleteMovie(Guid requestId);
        public bool CheckMovieExisting(Guid requestId);
        public bool CheckMovieExisting(string title);
        public int MinimumMovieDate();
        public int MaximumMovieDate();
    }
}
