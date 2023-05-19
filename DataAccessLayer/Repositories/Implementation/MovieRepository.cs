using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Models.Joins;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Implementation
{
    public class MovieRepository : IMovieRepository
    {
        private readonly CinemaContext _database;
        public MovieRepository(CinemaContext database)
        {
            _database = database;
        }

        public async Task AddMovieAsync(Movie request)
        {
            _database.Movies.Add(request);
            await _database.SaveChangesAsync();
        }

        public async Task AddMovieActorsAsync(List<MovieActor> movieActors)
        {
            _database.MoviesActors.AddRange(movieActors);
            await _database.SaveChangesAsync();
        }

        public async Task AddMovieGenresAsync(List<MovieGenre> movieGenres)
        {
            _database.MoviesGenres.AddRange(movieGenres);
            await _database.SaveChangesAsync();
        }

        public async Task ChangeMovieAsync(Movie request)
        {
            _database.Movies.Update(request);
            await _database.SaveChangesAsync();
        }

        public async Task DeleteMovie(Movie movie)
        {
            _database.Movies.Remove(movie);
            await _database.SaveChangesAsync();
        }

        public async Task DeleteMovieActorAsync(MovieActor request)
        {
            _database.MoviesActors.Remove(request);
            await _database.SaveChangesAsync();
        }

        public async Task DeleteMovieGenreAsync(MovieGenre request)
        {
            _database.MoviesGenres.Remove(request);
            await _database.SaveChangesAsync();
        }

        public async Task<Movie> GetMovieByIdAsync(Guid requestId)
        {
            var movie = await _database.Movies.Where(m => m.Id == requestId).FirstOrDefaultAsync();
            return movie;
        }

        public async Task<ICollection<Movie>> GetMoviesAsync()
        {
            var movies = await _database.Movies.ToListAsync();

            return movies;
        }

        public async Task<ICollection<MovieActor>> GetMovieActorsAsync()
        {
            var moviActors = await _database.MoviesActors.ToListAsync();
            return moviActors;
        }

        public async Task<ICollection<MovieGenre>> GetMovieGenresAsync()
        {
            var moviGenres = await _database.MoviesGenres.ToListAsync();
            return moviGenres;
        }
    }
}
