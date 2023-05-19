using DataAccessLayer.Models;
using DataAccessLayer.Models.Joins;

namespace DataAccessLayer.Repositories.Interfaces
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
