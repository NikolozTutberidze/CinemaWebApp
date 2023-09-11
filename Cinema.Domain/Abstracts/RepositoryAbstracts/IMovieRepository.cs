using Cinema.Domain.Models;
using Cinema.Domain.Models.Joins;

namespace Cinema.Domain.Abstracts.RepositoryAbstracts
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
