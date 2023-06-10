using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Models.Joins;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Implementation
{
    public class GenreRepository : IGenreRepository
    {
        private readonly CinemaContext _database;
        public GenreRepository(CinemaContext database)
        {
            _database = database;
        }
        public async Task AddGenreAsync(Genre request)
        {
            _database.Genres.Add(request);
            await _database.SaveChangesAsync();
        }

        public async Task ChangeGenreAsync(Genre request)
        {
            _database.Genres.Update(request);
            await _database.SaveChangesAsync();
        }

        public async Task DeleteGenreAsync(Genre request)
        {
            _database.Genres.Remove(request);
            await _database.SaveChangesAsync();
        }

        public async Task<Genre> GetGenreAsync(Guid requestId)
        {
            var genre = await _database.Genres.FirstOrDefaultAsync(g => g.Id == requestId);
            return genre;
        }

        public async Task<ICollection<Genre>> GetGenresAsync()
        {
            var genres = await _database.Genres.ToListAsync();
            return genres;
        }

        public async Task<ICollection<MovieGenre>> GetMovieGenresAsync()
        {
            return await _database.MoviesGenres.ToListAsync();
        }
    }
}
