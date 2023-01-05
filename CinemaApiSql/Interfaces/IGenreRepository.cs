using CinemaApiSql.Dtos;
using CinemaApiSql.Models;

namespace CinemaApiSql.Interfaces
{
    public interface IGenreRepository
    {
        public void AddGenre(Genre request);
        public GenreDto GetGenre(Guid requestId);
        public ICollection<GenreDto> GetGenres();
        public void ChangeGenre(Genre request);
        public void DeleteGenre(Guid requestId);
        public bool CheckGenreExisting(Guid requestId);
        public bool CheckGenreExisting(string name);

    }
}
