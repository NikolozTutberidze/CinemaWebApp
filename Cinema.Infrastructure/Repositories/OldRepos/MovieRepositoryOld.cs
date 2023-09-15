using Cinema.Domain.Abstracts.RepositoryAbstracts;
using Cinema.Domain.Models;
using Cinema.Domain.Models.Joins;
using Cinema.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Repositories.OldRepos
{
    public class MovieRepositoryOld : IMovieRepository
    {
        private readonly CinemaContext _database;
        public MovieRepositoryOld(CinemaContext database)
        {
            _database = database;
        }

        public async Task AddMovieAsync(Movie request)
        {
            _database.Movies.Add(request);
            await _database.SaveChangesAsync();
        }

        public void AddMovieActorsAsync(List<MovieActor> movieActors)
        {
            _database.MoviesActors.AddRange(movieActors);
        }

        public void AddMovieGenresAsync(List<MovieGenre> movieGenres)
        {
            _database.MoviesGenres.AddRange(movieGenres);
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

        public void DeleteMovieActorsAsync(List<MovieActor> request)
        {
            _database.MoviesActors.RemoveRange(request);
        }

        public void DeleteMovieGenresAsync(List<MovieGenre> request)
        {
            _database.MoviesGenres.RemoveRange(request);
        }

        public async Task<Movie> GetMovieByIdAsync(Guid requestId)
        {
            var movie = await _database.Movies.FirstOrDefaultAsync(m => m.Id == requestId);
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
