using DataAccessLayer.Models;
using DataAccessLayer.Models.Joins;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        Task AddMovieAsync(Movie request);
        void AddMovieActorsAsync(List<MovieActor> movieActors);
        void AddMovieGenresAsync(List<MovieGenre> movieGenres);
        Task<ICollection<Movie>> GetMoviesAsync();
        Task<Movie> GetMovieByIdAsync(Guid requestId);
        Task<ICollection<MovieActor>> GetMovieActorsAsync();
        Task<ICollection<MovieGenre>> GetMovieGenresAsync();
        Task ChangeMovieAsync(Movie request);
        Task DeleteMovie(Movie movie);
        void DeleteMovieActorsAsync(List<MovieActor> request);
        void DeleteMovieGenresAsync(List<MovieGenre> request);
    }
}
