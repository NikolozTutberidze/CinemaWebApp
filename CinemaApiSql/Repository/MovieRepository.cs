using CinemaApiSql.Data;
using CinemaApiSql.Dtos;
using CinemaApiSql.Interfaces;
using CinemaApiSql.Models;
using CinemaApiSql.Models.Joins;

namespace CinemaApiSql.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly CinemaContext _database;
        public MovieRepository(CinemaContext database)
        {
            _database = database;
        }

        public void AddMovie(AddMovieDto request)
        {

            Movie movie = new()
            {
                Title = request.Title,
                Date = request.Date,
                IMDBRate = request.IMDBRate,
                Review = request.Review,
                DirectorId = request.DirectorId,
            };

            List<MovieActor> movieActors = new();
            foreach (var actorId in request.ActorIds)
            {
                movieActors.Add(new MovieActor
                {
                    Actor = _database.Actors.Where(a => a.Id == actorId).First(),
                    Movie = movie
                });
            }
            _database.MoviesActors.AddRange(movieActors);

            List<MovieGenre> movieGenres = new();
            foreach (var genreId in request.GenreIds)
            {
                movieGenres.Add(new MovieGenre
                {
                    Genre = _database.Genres.Where(g => g.Id == genreId).First(),
                    Movie = movie
                });
            }
            _database.MoviesGenres.AddRange(movieGenres);

            _database.Movies.Add(movie);
            _database.SaveChanges();
        }

        public void ChangeMovie(ChangeMovieDto request)
        {
            Movie movie = new();
            movie.Id = request.Id;
            if (!(request.Title == null))
                movie.Title = request.Title;
            if (!(request.Date == null))
                movie.Date = request.Date;
            if (!(request.IMDBRate == null))
                movie.IMDBRate = (double)request.IMDBRate;
            if (!(request.Review == null))
                movie.Review = request.Review;
            if (!(request.DirectorId == null))
                movie.DirectorId = (Guid)request.DirectorId;

            List<MovieActor> movieActors = new();
            if (request.ActorIds.Count > 0)
            {
                foreach (var actorId in request.ActorIds)
                {
                    movieActors.Add(new MovieActor
                    {
                        Actor = _database.Actors.Where(a => a.Id == actorId).First(),
                        Movie = movie
                    });
                }
                foreach (var item in _database.MoviesActors)
                {
                    if (item.MovieId == movie.Id)
                        _database.MoviesActors.Remove(item);
                }
                _database.MoviesActors.AddRange(movieActors);
            }

            List<MovieGenre> movieGenres = new();
            if (request.GenreIds.Count > 0)
            {
                foreach (var genreId in request.GenreIds)
                {
                    movieGenres.Add(new MovieGenre
                    {
                        Genre = _database.Genres.Where(g => g.Id == genreId).First(),
                        Movie = movie
                    });
                }
                foreach (var item in _database.MoviesGenres)
                {
                    if (item.MovieId == movie.Id)
                        _database.MoviesGenres.Remove(item);
                }
                _database.MoviesGenres.AddRange(movieGenres);
            }

            _database.Movies.Update(movie);
            _database.SaveChanges();
        }

        public void DeleteMovie(Guid requestId)
        {
            var movie = _database.Movies.Where(m => m.Id == requestId).First();
            _database.Movies.Remove(movie);
            _database.SaveChanges();
        }

        public MovieDto GetMovieById(Guid requestId)
        {
            var movie = _database.Movies.Where(m => m.Id == requestId).Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Date = m.Date,
                IMDBRate = m.IMDBRate,
                Review = m.Review,
                DirectorId = m.DirectorId,
                MoviesActors = m.MoviesActors,
                MoviesGenres = m.MoviesGenres
            }).First();
            return movie;
        }

        public ICollection<MovieDto> GetMovieByIMDB(double firstRating, double secontRating)
        {
            double minRating = firstRating > secontRating ? secontRating : firstRating;
            double maxRating = firstRating == minRating ? secontRating : firstRating;
            var movies = _database.Movies.Where(m => m.IMDBRate >= minRating && m.IMDBRate <= maxRating).Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Date = m.Date,
                IMDBRate = m.IMDBRate,
                Review = m.Review,
                DirectorId = m.DirectorId,
                MoviesActors = m.MoviesActors,
                MoviesGenres = m.MoviesGenres
            }).ToList();
            return movies;

        }

        public ICollection<MovieDto> GetMovieByTitle(string requestTitle)
        {
            var movies = _database.Movies.Where(m => m.Title.ToUpper().Contains(requestTitle.ToUpper())).Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Date = m.Date,
                IMDBRate = m.IMDBRate,
                Review = m.Review,
                DirectorId = m.DirectorId,
                MoviesActors = m.MoviesActors,
                MoviesGenres = m.MoviesGenres
            }).ToList();
            return movies;
        }

        public ICollection<MovieDto> GetMovies()
        {
            var movies = _database.Movies.Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Date = m.Date,
                IMDBRate = m.IMDBRate,
                Review = m.Review,
                DirectorId = m.DirectorId,
                MoviesActors = m.MoviesActors,
                MoviesGenres = m.MoviesGenres
            }).ToList();

            return movies;
        }

        public ICollection<MovieDto> GetMovieByYear(int firstYear, int secondYear)
        {
            int minYear = firstYear > secondYear ? secondYear : firstYear;
            int maxYear = minYear == firstYear ? secondYear : firstYear;
            var movies = _database.Movies.Where(m => Convert.ToInt32(m.Date) >= minYear && Convert.ToInt32(m.Date) <= maxYear).Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Date = m.Date,
                IMDBRate = m.IMDBRate,
                Review = m.Review,
                DirectorId = m.DirectorId,
                MoviesActors = m.MoviesActors,
                MoviesGenres = m.MoviesGenres
            }).ToList();
            return movies;
        }

        public bool CheckMovieExisting(Guid requestId)
        {
            if (_database.Movies.Any(movie => movie.Id == requestId))
                return true;
            else
                return false;
        }

        public bool CheckMovieExisting(string title)
        {
            if (_database.Movies.Any(movie => movie.Title == title))
                return true;
            else
                return false;
        }

        public int MinimumMovieDate()
        {
            var minDate = _database.Movies.Min(m => m.Date);
            return Convert.ToInt32(minDate);
        }

        public int MaximumMovieDate()
        {
            var maxDate = _database.Movies.Max(m => m.Date);
            return Convert.ToInt32(maxDate);
        }
    }
}
