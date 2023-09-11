using Cinema.Domain.Models;
using Cinema.Domain.Models.Joins;

namespace Cinema.Domain.Abstracts.RepositoryAbstracts
{
    public interface IGenreRepository
    {
        Task AddGenreAsync(Genre request);
        Task<Genre> GetGenreAsync(Guid requestId);
        Task<ICollection<Genre>> GetGenresAsync();
        Task<ICollection<MovieGenre>> GetMovieGenresAsync();
        Task ChangeGenreAsync(Genre request);
        Task DeleteGenreAsync(Genre request);
    }
}
