using CinemaApiSql.Data;
using CinemaApiSql.Dtos;
using CinemaApiSql.Interfaces;
using CinemaApiSql.Models;

namespace CinemaApiSql.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly CinemaContext _database;
        public GenreRepository(CinemaContext database)
        {
            _database = database;
        }
        public void AddGenre(Genre request)
        {
            _database.Genres.Add(request);
            _database.SaveChanges();
        }

        public void ChangeGenre(Genre request)
        {
            _database.Genres.Update(request);
            _database.SaveChanges();
        }

        public bool CheckGenreExisting(Guid requestId)
        {
            if (_database.Genres.Any(g => g.Id == requestId))
                return true;
            else
                return false;
        }

        public bool CheckGenreExisting(string name)
        {
            if (_database.Genres.Any(g => g.Name == name))
                return true;
            else
                return false;
        }

        public void DeleteGenre(Guid requestId)
        {
            var genre = _database.Genres.Where(g => g.Id == requestId).First();
            _database.Genres.Remove(genre);
            _database.SaveChanges();
        }

        public GenreDto GetGenre(Guid requestId)
        {
            var genre = _database.Genres.Where(g => g.Id == requestId).Select(g => new GenreDto
            {
                Id = g.Id,
                Name = g.Name,
                MoviesGenres = g.MoviesGenres
            }).First();
            return genre;
        }

        public ICollection<GenreDto> GetGenres()
        {
            var genres = _database.Genres.Select(g => new GenreDto
            {
                Id = g.Id,
                Name = g.Name,
                MoviesGenres = g.MoviesGenres
            }).ToList();
            return genres;
        }
    }
}
